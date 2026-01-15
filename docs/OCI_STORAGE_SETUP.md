# Oracle Cloud Object Storage Configuration Guide

This guide explains how to configure Oracle Cloud Object Storage for the FastConsig application.

## Prerequisites

1. Oracle Cloud account with active tenancy
2. User with appropriate permissions to create and manage Object Storage buckets
3. API signing key pair generated for the user

## Setup Steps

### 1. Create Object Storage Bucket

1. Log in to Oracle Cloud Console
2. Navigate to **Storage > Object Storage > Buckets**
3. Create a new bucket with the following settings:
   - **Name**: `fastconsig-uploads` (or your preferred name)
   - **Storage Tier**: Standard
   - **Encryption**: Enable encryption (recommended)
   - **Versioning**: Optional (recommended for audit purposes)
   - **Visibility**: Private

### 2. Generate API Signing Key

1. Navigate to **Identity > Users**
2. Select your user
3. Under **API Keys**, click **Add API Key**
4. Download the private key (`.pem` file) and save it securely
5. Copy the fingerprint displayed after key creation

### 3. Collect Required Information

You'll need the following information from your Oracle Cloud account:

- **Tenancy OCID**: Found in **Tenancy Details**
- **User OCID**: Found in **User Settings**
- **Fingerprint**: From API key generation step
- **Private Key Path**: Location where you saved the `.pem` file
- **Region**: Your OCI region (e.g., `us-ashburn-1`)
- **Namespace**: Found in **Tenancy Details** under **Object Storage Namespace**
- **Bucket Name**: Name of the bucket you created
- **Compartment OCID**: OCID of the compartment containing the bucket

### 4. Configure Environment Variables

Update your `.env` file with the collected information:

```env
STORAGE_PROVIDER=oci

OCI_TENANCY_ID=ocid1.tenancy.oc1..aaaaaaaa...
OCI_USER_ID=ocid1.user.oc1..aaaaaaaa...
OCI_FINGERPRINT=aa:bb:cc:dd:ee:ff:00:11:22:33:44:55:66:77:88:99
OCI_PRIVATE_KEY_PATH=/path/to/oci_api_key.pem
OCI_REGION=us-ashburn-1
OCI_NAMESPACE=your_namespace
OCI_BUCKET_NAME=fastconsig-uploads
OCI_COMPARTMENT_ID=ocid1.compartment.oc1..aaaaaaaa...
```

### 5. Security Best Practices

1. **Never commit** the `.env` file or private key to version control
2. Store the private key in a secure location with restricted permissions:
   ```bash
   chmod 600 /path/to/oci_api_key.pem
   ```
3. Use OCI Vault for production secrets management
4. Rotate API keys periodically
5. Use IAM policies to restrict bucket access to specific users/groups

### 6. IAM Policy Configuration

Create an IAM policy to grant the necessary permissions:

```
Allow group FastConsigUsers to manage objects in compartment FastConsig where target.bucket.name='fastconsig-uploads'
Allow group FastConsigUsers to read buckets in compartment FastConsig
```

## Local Development

For local development, you can use the local storage provider:

```env
STORAGE_PROVIDER=local
LOCAL_STORAGE_PATH=./storage
```

This will store files in the `./storage` directory instead of uploading to OCI.

## Troubleshooting

### Authentication Errors

- Verify all OCIDs are correct
- Check that the fingerprint matches the API key
- Ensure the private key file path is accessible and has correct permissions

### Permission Errors

- Verify the user has the necessary IAM permissions
- Check that the bucket exists in the specified compartment
- Confirm the compartment OCID is correct

### Connection Errors

- Verify the region is correctly specified
- Check network connectivity to OCI endpoints
- Ensure no firewall rules are blocking outbound HTTPS traffic

## References

- [OCI Object Storage Documentation](https://docs.oracle.com/en-us/iaas/Content/Object/Concepts/objectstorageoverview.htm)
- [OCI SDK for Node.js](https://docs.oracle.com/en-us/iaas/Content/API/SDKDocs/typescriptsdk.htm)
- [OCI IAM Policies](https://docs.oracle.com/en-us/iaas/Content/Identity/Concepts/policygetstarted.htm)
