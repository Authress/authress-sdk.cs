# Authress.SDK.DTO.ClientAccessKey
## Properties

Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**KeyId** | **string** | The unique id of the client. | [optional]
**ClientId** | **string** | The unique id of the client. |
**GenerationDate** | **DateTime?** |  | [optional]
**AccessKey** | **string** | An encoded access key which contains identifying information for client access token creation. For direct use with the Authress SDKs, not meant to be decoded. Must be saved on creted, as it will never be returned from the API ever again. Authress only saved the corresponding public key to verify private access keys. | [optional]

[[Back to Model list]](../README.md#documentation-for-models) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to README]](../README.md)

