# Authress.SDK.DTO.PermissionObject
## Properties

Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**Action** | **string** | The action the permission grants, can be scoped using &#x60;:&#x60; and parent actions imply child permissions, action:* or action implies action:sub-action. |
**Allow** | **bool?** | Does this permission grant the user the ability to execute the action? |
**Grant** | **bool?** | Allows the user to give the permission to others without being able to execute the action. |
**Delegate** | **bool?** | Allows delegating or granting the permission to others without being able to execute tha action. |

[[Back to Model list]](../README.md#documentation-for-models) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to README]](../README.md)

