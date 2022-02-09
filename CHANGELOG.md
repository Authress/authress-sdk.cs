# Change log
This is the changelog for [Authress SDK](readme.md).

## 2.0 ##
* Renamed `AccessRecordStatements` and other models that end with `S` but aren't actually plural to be `AccessRecordStatement` (without the `S`).
* Use typed enum `Status` for method inputs instead of string values.

## 1.2 ##
* Upgrade to using core 3.1.
* Added `AccessRequests` and `Invites` methods to `RecordsApi` interface.

## 1.1 ##
* The `GetUserResources` api now provides a `permission` argument to filter returned results based on this permission. There are two changes to the is method. When a user has access to all sub-resources `Resources` will be `null` and `AccessToAllSubResources` will be `true`. When a user does not have access to all sub-resources `Resources` will contain the list of sub-resources they do have that permission to. The API no longer returns an single element `[CollectionResourceUri]` in the case where the user has access to all sub-resources. To prevent unexpected side-effects in services, `Resources` will be null so an error will be generated.
* Added `AccessRecordGroups` to `AccessRecords`
* Added Invites endpoints
