#!/bin/bash

# Set variables
REGISTRY_NAME="myregistry.azurecr.io" # replace with container registry name here
SERVICES=("user-service" "task-service" "notification-service" "analytics-service")
DEPLOYMENTS_DIR="./deployments"

# Update deployment files with the registry name
for SERVICE in "${SERVICES[@]}"; do
  echo "Updating deployment file for $SERVICE..."
  sed -i "s|<registry-name>|$REGISTRY_NAME|g" $DEPLOYMENTS_DIR/$SERVICE-deployment.yaml
done

# Build and push Docker images for all services
for SERVICE in "${SERVICES[@]}"; do
  echo "Building Docker image for $SERVICE..."
  docker build -t $REGISTRY_NAME/$SERVICE:latest ./$SERVICE

  echo "Pushing Docker image for $SERVICE to $REGISTRY_NAME..."
  docker push $REGISTRY_NAME/$SERVICE:latest
done

# Apply Kubernetes deployment files
echo "Applying Kubernetes deployment files..."
kubectl apply -f $DEPLOYMENTS_DIR/

echo "Deployment completed successfully!"
