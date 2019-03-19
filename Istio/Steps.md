# Steps to operate Istio on AKS

1. Create an Azure Resource Group
2. Create a Service Principal and give it access to the resource group
3. Create an Azure Container Registry in the Resource Group
4. Create a VNet in the Azure Resource Group, which is will be used by Azure Kubernetes Service
5. Create an Azure Kubernetes Service cluster
6. Install azure cli locally
7. Install kubectl locally
8. Install Helm locally
9. Install Docker locally
10. az login
11. login to acr
12. Build your dotnet core code locally using cli
13. Create docker file for your dotnet core code
14. Docker build your code locally
15. Tag the docker image built in previous step
16. Push the docker image to the ACR
17. Create a deployment manifest for you dotnet core microservice
18. Point your local kubectl to AKS cluster created using get credentials
19. kubectl apply your deployment manifest
20. Verify the micro service is running as expected
21. Show how there is only 1/1 in the pods
22. Create the RBAC and apply it to get Tiller going
23. Helm init to ensure Helm and Tiller are running
24. Make sure your helm has matching client and server versions
25. Get Istio code locally
26. Install Istio using helm
    1. Navigate to root of istio-1.0.3 and execute the below command
    2. helm template install/kubernetes/helm/istio --name istio --namespace istio-system --set ingress.enabled=true --set grafana.enabled=true --set servicegraph.enabled=true --set tracing.enabled=true --set kiali.enabled=true > $HOME/istio.yaml
27. Ensure you turn on Istio on the namespace where you want to install your microservices. This enables automatic sidecar injection
28. Just delete and redeploy your microservice. You will now see 2/2 and side car is now injected.
29. However you cannot access the service from outside yet. So you will have to enable it using Istio gateway
30. Install certmanager using helm and tiller --> this goes into kube-system namespace and seves entire kubernetes cluster
31. Create Cluster certificate Issuer using manifest.yaml
32. Create a Certificate object using manifest.yaml --> this goes into the namespace where you deploy your services... microservices namespace in this case.
33. Create the certificates using instructions in Istio site https://istio.io/docs/tasks/traffic-management/secure-ingress/  --> this just creates the certs.
34. Create a secret to hold certs in kubernetes --> this creates scerets for cert in the namespace istio-system
kubectl create -n istio-system secret tls istio-ingressgateway-certs --key *.smanyamr.com/3_application/private/*.smanyamr.com.key.pem --cert *.smanyamr.com/3_application/certs/*.smanyamr.com.cert.pem
35. Map your istio-ingressgateway external ip address from kubectl get services -n istio-system to A records in your domain like GoDaddy

kubectl port-forward -n istio-system $(kubectl get pod -n istio-system -l app=jaeger -o jsonpath='{.items[0].metadata.name}') 16686:16686 &


kubectl port-forward -n istio-system $(kubectl get pod -n istio-system -l app=zipkin -o jsonpath='{.items[0].metadata.name}') 9411:9411 &