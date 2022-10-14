using Amazon.CDK;
using Amazon.CDK.AWS.S3;
using Amazon.CDK.AWS.S3.Deployment;
using Constructs;

namespace Cloudfrontmultiregs3;

public class PrimaryRegionStackProps : StackProps
{
    public string PathToAssets { get; set; }
}

public class PrimaryRegionStack : Stack
{
    public IBucket Bucket { get; private set; }
    internal PrimaryRegionStack(Construct scope, string id, PrimaryRegionStackProps props) : base(scope, id, props)
    {
        Bucket = new Bucket(this, "Bucket", new BucketProps
        {
            BucketName = PhysicalName.GENERATE_IF_NEEDED
        });
        Bucket.ApplyRemovalPolicy(RemovalPolicy.DESTROY);

        new BucketDeployment(this, "BucketDeployment", new BucketDeploymentProps
        {
            DestinationBucket = Bucket,
            Sources = new[]
            {
                Source.Asset(props.PathToAssets)
            }
        });
    }
}
