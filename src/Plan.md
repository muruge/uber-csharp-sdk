# Map out all endpoints

/v1/requests/{request_id}/map only returns map when you're on a trip
{"message":"You are not currently on a trip.","code":"conflict"}

/v1/requests/{request_id} only returns ETA, driver and vehicle if you're on a trip

/v1/requests does not show current processing requests