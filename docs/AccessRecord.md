# Authress.SDK.DTO.AccessRecord
## Properties

Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**RecordId** | **string** |  |
**Name** | **string** | A helpful name for this record |
**Account** | [**AccessRecordAccount**](AccessRecordAccount.md) |  |
**Users** | [**List&lt;AccessRecordUsers&gt;**](AccessRecordUsers.md) | The list of users this record applies to |
**Admins** | [**List&lt;AccessRecordUsers&gt;**](AccessRecordUsers.md) | The list of admin that can edit this record even if they do not have global record edit permissions. |
**Statements** | [**List&lt;AccessRecordStatements&gt;**](AccessRecordStatements.md) | A list of statements which match roles to resources. Users in this record have all statements apply to them |
**Links** | **Object** |  |

[[Back to Model list]](../README.md#documentation-for-models) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to README]](../README.md)

