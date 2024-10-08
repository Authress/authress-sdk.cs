# Change log
This is the changelog for [Authress SDK](readme.md).

## 2.0 ##
* `ApiBasePath` has been renamed to `AuthressApiUrl`.
* `HttpClientSettings` Has been removed in favor of `AuthressSettings` Class.
* [Breaking] `UserPermissions.GetUserResources()` no longer returns the property `AccessToAllSubResources`. When a user only has access to parent resources, the list will always be empty unless the `CollectionConfigurationEnum` property is specified.
* [Breaking] Renamed `AccessRecordStatements` and other models that end with `S` but aren't actually plural to be `AccessRecordStatement` (without the `S`).

## 1.5 ##
* Fix `DateTimeOffset` type assignments, properties that were incorrectly defined as `DateTime` are now correctly `DateTimeOffsets`.
* Add in `VerifyToken()` method to `AuthressClient`..
* Fix `Invite` Statement usage to use dedicated InviteStatement.

## 1.4 ##
* Support exponential back-off retries on unexpected failures.
* Add optimized caching for authorization checks
* Add support for If-Unmodified-Since in group and access record apis.

## 1.3 ##
* Upgrade to using dotnet 6.0 support.
* Added `CollectionConfigurationEnum` for the GET User Resources endpoint.
* Automatically inject in the authress custom domain to the token providers.


## 1.2 ##
* Upgrade to using core 3.1.
* Added `AccessRequests` and `Invites` methods to `RecordsApi` interface.

## 1.1 ##
* The `GetUserResources` api now provides a `permission` argument to filter returned results based on this permission. There are two changes to the is method. When a user has access to all sub-resources `Resources` will be `null` and `AccessToAllSubResources` will be `true`. When a user does not have access to all sub-resources `Resources` will contain the list of sub-resources they do have that permission to. The API no longer returns an single element `[CollectionResourceUri]` in the case where the user has access to all sub-resources. To prevent unexpected side-effects in services, `Resources` will be null so an error will be generated.
* Added `AccessRecordGroups` to `AccessRecords`
* Added Invites endpoints
