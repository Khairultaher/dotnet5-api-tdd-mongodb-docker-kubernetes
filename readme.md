## Build + Publish ==> Deployment ==> Service Config
### Build image and run in docker
```
0. Configure docker file (Dockerfile)
1. Build image: 
docker build -t catalog:v1 .
OR 
docker build -t khairul100/catalog:v1 .
2. Create network to connect to other container: 
docker network ls (for see the list of network)
docker network rm name_of_network (for deleting the network)
docker network create catalog-net
3.Pull & Run mongodb container:
docker ps (to check mongo is running)
docker stop name_of_container (if it is already running) 
docker run -d --rm --name mongo -p 27018:27017 -v mongodbdata:/data/db -e MONGO_INITDB_ROOT_USERNAME=mongoadmin -e MONGO_INITDB_ROOT_PASSWORD=admin --network=catalog-net mongo
4. Run tagged image & Create container: 
docker run --rm -p 8080:80 -e MongoDbSettings:Host=mongo -e MongoDbSettings:Password=admin --network=catalog-net khairul100/catalog:v1 .
5. Check images and containers
docker images
docker ps
6. Push image:
docker push khairul100/catalog:v1
7. Pull image:
docker pull khairul100/catalog:v1
OR
Run command available in no. 4
```

### Deployment in Kubenetes
```
1. Check Kubenetes up and running:
kubectl config current-context
2. Create & Setup secret on kubernetes:
kubectl create secret generic catalog-secrets --from-literal=mongodb-password='admin'
3. Create & configure pod deployment yaml file(catalog.yaml)
4. Create & configure service externaly or internaly(service.yaml file or add end of the deployment file) 
5. Deploy pod & check status:
kubectl apply -f catalog.yaml
kubectl get deployments
kubectl get pods
kubectl logs created_pod_name_here
6. Create & configure database(mongodb) deployment yaml file(mongodb.yaml)
7. Deploy database & check staus(from directory here the yaml file is available)
kubectl apply -f mongodb.yaml
kubectl get statefulsets
kubectl get pods or kubectl get pods -w
```
### Clean up all pods, services & secrets
```
kubectl delete deployment catalog-deployment
kubectl delete pod catalog-deployment-69bc754648-6nmlm
kubectl delete pod mongodb-statefulset-0

kubectl delete statefulset mongodb-statefulset
kubectl delete svc mongodb-service
kubectl delete svc catalog-service
kubectl delete svc mongodb-statefulset-0
kubectl delete secret catalog-secrets
```
