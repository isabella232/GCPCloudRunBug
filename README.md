# GCPCloudRunBug
Repo to document &amp; reproduce GCP web console admin credential / environment variable bug

## Installation/Setup

Install instructions 

```bash
# install/setup commands
```

## Usage

```yaml
      containers:
      - image: image.path
        ports:
        - name: http1
          containerPort: 8080
        env:
        - name: '''GOOGLE_APPLICATION_CREDENTIALS'''
          valueFrom:
            secretKeyRef:
              key: latest
              name: GOOGLE_APPLICATION_CREDENTIALS
        resources:
          limits:
            cpu: 1000m
            memory: 512Mi
```


## License
[MIT](https://choosealicense.com/licenses/mit/)
