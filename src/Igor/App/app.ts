module Igor {
    export interface WebSiteDto {
        name: string;
        hostName: string;
        webSpace: string;
        serverFarm: string;
        deploymentMessage?: string;
        swapStatus?: string;
    }
    export interface DeploymentDto {
        message: string;
        time: string;
    }
    export interface SwapResultDto {
        operationId: string;
    }
    export interface OperationStatusDto {
        status: string;
    }
    export interface DtoCollection<T> {
        items: T[];
    }
    export interface IAlert {
        type: string;
        message: string;
        closeable?: boolean;
    }
    export class MainCtrl {
        public title = "Igor";
        public sites: WebSiteDto[];
        public alerts: IAlert[];
        public runningOperations: number = 0;
        
        constructor(private $http: ng.IHttpService, private $timeout: ng.ITimeoutService, private $window: ng.IWindowService) {
            this.loadWebsites();
        }

        loadWebsites() {
            this.runningOperations++;
            var loadingAlert = { type: "info", message: "Loading sites..." };
            this.alerts = [loadingAlert];
            this.$http.get("/websites")
                .success((dto: DtoCollection<WebSiteDto>) => {
                    this.sites = dto.items;
                    this.alerts.remove(loadingAlert);
                    this.loadDeployments();
                })
                .error(() => {
                    this.alerts.remove(loadingAlert);
                    this.alerts.push({ type: "danger", message: "Failed to load sites.", closeable: true });
                })
                .finally(() => {
                    this.runningOperations--;
                });
        }

        loadDeployments() {
            this.sites.forEach((site) => {
                if (site.name.endsWith("(staging)")) {
                    this.loadDeployment(site);
                }
            });
        }

        loadDeployment(site: WebSiteDto) {
            this.runningOperations++;
            site.deploymentMessage = "(Loading...)";
            this.$http.get("/websites/" + site.webSpace + "/" + site.hostName + "/deployment")
                .success((dto: DeploymentDto) => {
                    site.deploymentMessage = dto.message;
                })
                .error(() => {
                    site.deploymentMessage = "[No current deployment found]";
                })
                .finally(() => {
                    this.runningOperations--;
                });
        }

        swap() {
            this.sites.forEach((site) => {
                if (!site.name.endsWith("(staging)")) {
                    this.runningOperations++;
                    var url = "/websites/" + site.webSpace + "/" + site.name + "/swap/staging";
                    this.$http.post(url, site)
                        .success((dto: SwapResultDto) => {
                            this.checkOperationStatus(site, dto.operationId);
                        })
                        .error((data, status) => {
                            this.$window.alert(url + " returned " + status);
                        });
                }
            });
        }

        checkOperationStatus(site: WebSiteDto, operationId: string) {
            var url = "/websites/" + site.webSpace + "/" + site.name + "/operations/" + operationId;
            this.$http.get(url)
                .success((dto: OperationStatusDto) => {
                    site.swapStatus = dto.status;
                })
                .finally(() => {
                    if (site.swapStatus === "InProgress") {
                        this.$timeout(() => this.checkOperationStatus(site, operationId), 2000);
                    } else {
                        this.runningOperations--;
                        if (this.runningOperations === 0) {
                            this.loadDeployments();
                        }
                    }
                });
        }

        public closeAlert(index: number) {
            this.alerts.removeAt(index);
        }
    }

    angular.module("igor", ["ui.bootstrap","ui.grid"]);
}