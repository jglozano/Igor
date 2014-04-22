# Igor

> A little site for managing Azure Web Sites in bulk

## Who is it for?

Anybody who is running a site in more than one data centre, like we do at [Zudio](http://zud.io), deploying from source control, and using staging slots during deployment.

When we deploy, we deploy into staging slots on 10 sites in 10 data centres, and when that process is complete, we need to do a Slot Swap across all 10 of those sites simultaneously. So we wrote Igor to do that for us, and this is it.

## Usage

Igor is designed to run in an Azure Web Role. Ironic, but necessary because the Azure Management Libraries require an X509 certificate for authentication, and it's not currently possible to instantiate those programmatically in Azure Web Sites.

### Compiling and running

* Fork and clone this repository.
* Use the `.sample` files in the Igor.Azure folder to create the `ServiceDefinition.csdef` and the `ServiceConfiguration.*.cscfg` files. The originals are not committed to the repo because they contain our credentials.
* There are 3 settings in the `.cscfg` files:
    * **WebSites** should be a comma-separated list of sites for Igor to work with.
    * **SubscriptionId** is the GUID ID for your Azure subscription, in the format `00000000-0000-0000-0000-000000000000`
    * **ManagementCertificate** is the base-64 encoded representation of a management certificate for the subscription, which you can get from a Publish Settings file. You can download one here: https://windows.azure.com/download/publishprofile.aspx

Once you've created the settings files, you should be able to run the site locally in the emulator, and upload it to a Cloud Service if you want.

### Using the app

When you browse to the page, you will be prompted to log in. The user name and password are the credentials you set up for deployment of your Azure Web Sites, the same ones you use to log into the SCM console. The app uses the Web Sites service to validate the credentials, so it doesn't have to store them anywhere.

After logging in, you'll see a list of sites, with the staging slot sites showing their current deployment. At Zudio, we use the name `staging` for all our staging slots, and that's currently hard-coded into Igor. If your convention differs, just track that word down and change it. If you haven't set up staging slots yet, then when you do, call them `staging`. It's just easier that way.

If the current deployment is the one you've just pushed on all the sites, you can click the massive Swap button and it will swap all the sites for you. If any are lagging behind, there's a Refresh button you can click compulsively every five seconds until they're ready.

## Tech

Igor was built using:

* [The Windows Azure Management Libraries](https://github.com/Azure/azure-sdk-for-net)
* [Simple.Web](https://github.com/markrendle/simple.web) & [Simple.Owin](https://github.com/simple-owin)
* [AngularJS](http://angularjs.org)
* [Twitter Bootstrap](http://getbootstrap.com)
* [Angular UI](http://angular-ui.github.io/)
* [Ninject](http://www.ninject.org/)
* [SugarJS](http://sugarjs.com/)
* [TypeScript](http://www.typescriptlang.org/)
    * with definition files from [DefinitelyTyped](https://github.com/borisyankov/DefinitelyTyped)

Thank you to all those projects.

## Roadmap

We'll be adding a few new operations that can be run across multiple sites, including:

* Stop all WebJobs on staging sites
* Re-sync or re-deploy all staging sites
* Anything else we need

If you add any functionality to your fork of Igor that might be useful to the community, please do send us a Pull Request. Report any problems in the Issues section here.

## Notes

The current deployment for the Production slot is not shown because at present the Web Sites API won't actually return it, probably because the deployments are configured against the staging slot. If this changes we'll update the code.