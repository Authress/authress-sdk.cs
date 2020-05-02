
Class | Method | HTTP request | Description
------------ | ------------- | ------------- | -------------
*AccessRecordsApi* | [**V1ClaimsPost**](./AccessRecordsApi.md#v1claimspost) | **POST** /v1/claims | Claim a resource by an allowed user
*AccessRecordsApi* | [**V1RecordsGet**](./AccessRecordsApi.md#v1recordsget) | **GET** /v1/records | Get all account records.
*AccessRecordsApi* | [**V1RecordsPost**](./AccessRecordsApi.md#v1recordspost) | **POST** /v1/records | Create a new access record
*AccessRecordsApi* | [**V1RecordsRecordIdDelete**](./AccessRecordsApi.md#v1recordsrecordiddelete) | **DELETE** /v1/records/{recordId} | Deletes an access record.
*AccessRecordsApi* | [**V1RecordsRecordIdGet**](./AccessRecordsApi.md#v1recordsrecordidget) | **GET** /v1/records/{recordId} | Get an access record for the account.
*AccessRecordsApi* | [**V1RecordsRecordIdPut**](./AccessRecordsApi.md#v1recordsrecordidput) | **PUT** /v1/records/{recordId} | Update an access record.
*AccountsApi* | [**V1AccountsAccountIdGet**](./AccountsApi.md#v1accountsaccountidget) | **GET** /v1/accounts/{accountId} | Get account information.
*AccountsApi* | [**V1AccountsGet**](./AccountsApi.md#v1accountsget) | **GET** /v1/accounts | Get all accounts user has access to
*AccountsApi* | [**V1IdentitiesGet**](./AccountsApi.md#v1identitiesget) | **GET** /v1/identities | Get all linked identities for this account.
*AccountsApi* | [**V1IdentitiesPost**](./AccountsApi.md#v1identitiespost) | **POST** /v1/identities | Link a new account identity.
*ClientsApi* | [**V1ClientsClientIdAccessKeysKeyIdDelete**](./ClientsApi.md#v1clientsclientidaccesskeyskeyiddelete) | **DELETE** /v1/clients/{clientId}/access-keys/{keyId} | Remove an access key for a client
*ClientsApi* | [**V1ClientsClientIdAccessKeysPost**](./ClientsApi.md#v1clientsclientidaccesskeyspost) | **POST** /v1/clients/{clientId}/access-keys | Request a new access key
*ClientsApi* | [**V1ClientsClientIdDelete**](./ClientsApi.md#v1clientsclientiddelete) | **DELETE** /v1/clients/{clientId} | Delete a client
*ClientsApi* | [**V1ClientsClientIdGet**](./ClientsApi.md#v1clientsclientidget) | **GET** /v1/clients/{clientId} | Get a client.
*ClientsApi* | [**V1ClientsClientIdPut**](./ClientsApi.md#v1clientsclientidput) | **PUT** /v1/clients/{clientId} | Update a client
*ClientsApi* | [**V1ClientsGet**](./ClientsApi.md#v1clientsget) | **GET** /v1/clients | Get clients collection
*ClientsApi* | [**V1ClientsPost**](./ClientsApi.md#v1clientspost) | **POST** /v1/clients | Create a new client.
*ResourcePermissionsApi* | [**V1ResourcesGet**](./ResourcePermissionsApi.md#v1resourcesget) | **GET** /v1/resources | List resource configurations
*ResourcePermissionsApi* | [**V1ResourcesResourceUriGet**](./ResourcePermissionsApi.md#v1resourcesresourceuriget) | **GET** /v1/resources/{resourceUri} | Get a resource permissions object.
*ResourcePermissionsApi* | [**V1ResourcesResourceUriPut**](./ResourcePermissionsApi.md#v1resourcesresourceuriput) | **PUT** /v1/resources/{resourceUri} | Update a resource permissions object.
*UserPermissionsApi* | [**V1UsersUserIdResourcesResourceUriPermissionsGet**](./UserPermissionsApi.md#v1usersuseridresourcesresourceuripermissionsget) | **GET** /v1/users/{userId}/resources/{resourceUri}/permissions | Get the permissions a user has to a resource.
*UserPermissionsApi* | [**V1UsersUserIdResourcesResourceUriPermissionsPermissionGet**](./UserPermissionsApi.md#v1usersuseridresourcesresourceuripermissionspermissionget) | **GET** /v1/users/{userId}/resources/{resourceUri}/permissions/{permission} | Check to see if a user has permissions to a resource.

<a name="documentation-for-models"></a>
## Documentation for Models

 - [AccessRecord](./AccessRecord.md)
 - [AccessRecordAccount](./AccessRecordAccount.md)
 - [AccessRecordResources](./AccessRecordResources.md)
 - [AccessRecordStatements](./AccessRecordStatements.md)
 - [AccessRecordUsers](./AccessRecordUsers.md)
 - [Account](./Account.md)
 - [AccountCollection](./AccountCollection.md)
 - [ClaimRequest](./ClaimRequest.md)
 - [ClaimResponse](./ClaimResponse.md)
 - [Client](./Client.md)
 - [ClientAccessKey](./ClientAccessKey.md)
 - [ClientCollection](./ClientCollection.md)
 - [Identity](./Identity.md)
 - [IdentityCollection](./IdentityCollection.md)
 - [IdentityRequest](./IdentityRequest.md)
 - [PermissionObject](./PermissionObject.md)
 - [ResourcePermission](./ResourcePermission.md)
 - [ResourcePermissionCollection](./ResourcePermissionCollection.md)
 - [ResourcePermissionPermissions](./ResourcePermissionPermissions.md)
