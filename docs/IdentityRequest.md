# Authress.SDK.DTO.IdentityRequest
## Properties

Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**Jwt** | **string** | A valid JWT OIDC compliant token which will still pass authentication requests to the identity provider. Must contain a unique audience and issuer. |
**PreferredAudience** | **string** | If the &#x60;jwt&#x60; token contains more than one valid audience, then the single audience that should associated with Authress. If more than one audience is preferred, repeat this call with each one. | [optional]

[[Back to Model list]](../README.md#documentation-for-models) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to README]](../README.md)

