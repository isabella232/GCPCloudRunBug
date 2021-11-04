# GCPCloudRunBug

## Summary - tl;dr

Repo to document &amp; reproduce GCP web console admin credential / environment variable bug

## Installation/Setup

Install instructions

```bash
# install/setup commands
```

```bash
'/app{object}'
```

## Bugged Config

###### Screenshot here - Secret Manager Env Var config, No Quote

![Bugged Secrets Setup](https://github.com/stirista/GCPCloudRunBug/blob/main/images/CloudRun_Bugged.png))
#### Bugged Config YAML Output

```yaml
      containers:
      - image: image.path
        ports:
        - name: http1
          containerPort: 8080
        env:
        - name: GOOGLE_APPLICATION_CREDENTIALS
          valueFrom:
            secretKeyRef:
              key: latest
              name: GOOGLE_APPLICATION_CREDENTIALS
        resources:
          limits:
            cpu: 1000m
            memory: 512Mi
```

This config incorrectly formats the G_A_C environment variable key value as a "mounted volume" path string adding the container app root '/app/' to the beginning of the service account's json auth {object} string so that the value ultimately becomes '/app/{object}' which throws a runtime error anytime a GCP library attempts to authenticate

The runtime error, depending on the length of the keys in the {object} string is either a "path too long" or "path not found" exception

The exception handling for this error sprays the plaintext contents of the json G_A_C authentication {object} into the logs including authentication keys

![Bugged Secrets Error Msg](https://github.com/stirista/GCPCloudRunBug/blob/main/images/CloudRun_Bugged_Error.png)

## Working Config #1

###### Screenshot here - Secret Manager Env Var config, leading space no quote key

#### Working Config #1 YAML Output

```yaml
      containers:
      - image: image.path
        ports:
        - name: http1
          containerPort: 8080
        env:
        - name: ' GOOGLE_APPLICATION_CREDENTIALS'
          valueFrom:
            secretKeyRef:
              key: latest
              name: GOOGLE_APPLICATION_CREDENTIALS
        resources:
          limits:
            cpu: 1000m
            memory: 512Mi
```

## Working Config #2

###### Screenshot here - Secret Manager Env Var config, Single Quote key

#### Working Config #2 YAML Output

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

## Working Config #3

###### Screenshot here - No Secret Manager, Cloud Run Env Var config, No Quote key

#### Working Config #3 YAML Output

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
