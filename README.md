# bits-pilani-scalabale-services-assignment

I am building a To-Do List Application.
Define Services:

User Service: Manages user accounts and authentication.
Task Service: Handles creation, updates, and deletion of tasks.
Notification Service: Sends reminders and notifications for due tasks.
Analytics Service: Tracks task completion statistics and user activity.

## Deployment

The `deploy.sh` script is used for building Docker images for all services, pushing them to the container registry, and deploying them to Kubernetes. Ensure you have configured the script with the correct container registry name and deployment file paths.