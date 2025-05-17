# EzStocks

## Generate AppGateway certificate

- Created a self-sign cert using [appgwbackendcertgenerator](https://appgwbackendcertgenerator.azurewebsites.net/) for the public IP of the AppGateway.
- Uploaded the cert to the `Listener TLS certificates` within the portal
- Assigned the uploaded cert to the https listener
