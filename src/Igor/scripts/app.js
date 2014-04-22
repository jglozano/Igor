var Igor;
(function (Igor) {
    var MainCtrl = (function () {
        function MainCtrl($http, $timeout, $window) {
            this.$http = $http;
            this.$timeout = $timeout;
            this.$window = $window;
            this.title = "Igor";
            this.runningOperations = 0;
            this.loadWebsites();
        }
        MainCtrl.prototype.loadWebsites = function () {
            var _this = this;
            this.runningOperations++;
            var loadingAlert = { type: "info", message: "Loading sites..." };
            this.alerts = [loadingAlert];
            this.$http.get("/websites").success(function (dto) {
                _this.sites = dto.items;
                _this.alerts.remove(loadingAlert);
                _this.loadDeployments();
            }).error(function () {
                _this.alerts.remove(loadingAlert);
                _this.alerts.push({ type: "danger", message: "Failed to load sites.", closeable: true });
            }).finally(function () {
                _this.runningOperations--;
            });
        };

        MainCtrl.prototype.loadDeployments = function () {
            var _this = this;
            this.sites.forEach(function (site) {
                if (site.name.endsWith("(staging)")) {
                    _this.loadDeployment(site);
                }
            });
        };

        MainCtrl.prototype.loadDeployment = function (site) {
            var _this = this;
            this.runningOperations++;
            site.deploymentMessage = "(Loading...)";
            this.$http.get("/websites/" + site.webSpace + "/" + site.hostName + "/deployment").success(function (dto) {
                site.deploymentMessage = dto.message;
            }).error(function () {
                site.deploymentMessage = "[No current deployment found]";
            }).finally(function () {
                _this.runningOperations--;
            });
        };

        MainCtrl.prototype.swap = function () {
            var _this = this;
            this.sites.forEach(function (site) {
                if (!site.name.endsWith("(staging)")) {
                    _this.runningOperations++;
                    var url = "/websites/" + site.webSpace + "/" + site.name + "/swap/staging";
                    _this.$http.post(url, site).success(function (dto) {
                        _this.checkOperationStatus(site, dto.operationId);
                    }).error(function (data, status) {
                        _this.$window.alert(url + " returned " + status);
                    });
                }
            });
        };

        MainCtrl.prototype.checkOperationStatus = function (site, operationId) {
            var _this = this;
            var url = "/websites/" + site.webSpace + "/" + site.name + "/operations/" + operationId;
            this.$http.get(url).success(function (dto) {
                site.swapStatus = dto.status;
            }).finally(function () {
                if (site.swapStatus === "InProgress") {
                    _this.$timeout(function () {
                        return _this.checkOperationStatus(site, operationId);
                    }, 2000);
                } else {
                    _this.runningOperations--;
                    if (_this.runningOperations === 0) {
                        _this.loadDeployments();
                    }
                }
            });
        };

        MainCtrl.prototype.closeAlert = function (index) {
            this.alerts.removeAt(index);
        };
        return MainCtrl;
    })();
    Igor.MainCtrl = MainCtrl;

    angular.module("igor", ["ui.bootstrap", "ui.grid"]);
})(Igor || (Igor = {}));
//# sourceMappingURL=app.js.map
