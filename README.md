# Azure GitOps Service

This is the start of a proof of concept for a GitOps service for deploying Azure Templates. Right now, it simply reads a Template and Parameters file from Blob storage, then executes a WhatIf deployment API call against them. If WhatIf returns that changes would be made, then it will start a real deployment to correct the changes needed.

This is currently written as a WebJob SDK Queue Trigger. There is an accompanying REST API that is not part of the is repo.