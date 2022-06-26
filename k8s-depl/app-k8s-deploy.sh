#!/bin/bash

# DO NOT RUN THIS SCRIPT. INSTEAD, COPY EACH COMMAND TO A TERMINAL CONSOLE AND RUN IT
# IN ORDER.

# use this command to start minikube for rabbitmq cluster
minikube start --cpus=2 --memory=2040 --disk-size "10 GB"

# deploy app configmap & secrets
kubectl apply -f app-configmap.yml
kubectl apply -f app-secret.yml

# deploy rabbitmq configmap
kubectl apply -f rabbitmq-configmap.yml

# set up rabbitmq permissions
kubectl apply -f rabbitmq-rbac.yml

# deploy the rabbitmq sts
kubectl apply -f rabbitmq-statefulset.yml

# wait until rabbitmq pod is ready. Run "minikube service" command to get the management portal URL.
# Open the portal in browser (credential: guest/guest)
minikube service rabbitmq

# deploy sqlserver2019
kubectl apply -f mssql-depl.yml

# wait until sqlserver is running. Check the pod logs to verify.
kubectl logs <sqlserver-pod>

# deploy app
kubectl apply -f app-depl.yml

# check app pod logs to verify
kubectl logs <app-pod>

# get the app URL
minikube service unityapp-service

# open the swagger UI in browser using the ipaddress and port from above command
http://<ipaddress:port>/swagger

# test the app!
