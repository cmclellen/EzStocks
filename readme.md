# EzStocks

## AppGateway certificate

- Create a self-signed certificate in KeyVault using a domain name (don't use IP as that results in an ERR_SSL_UNRECOGNIZED_NAME_ALERT brower error).
  ![self-signed certificate](Docs/Images/self-signed-certificate.png)
- Reference the certificate created in the `Listeners` created.
