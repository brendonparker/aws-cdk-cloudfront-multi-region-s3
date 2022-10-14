using Amazon.CDK;
using Amazon.CDK.AWS.S3;
using Constructs;

namespace Cloudfrontmultiregs3;

public class PrimaryRegionStack : Stack
{
    public IBucket Bucket { get; private set; }
    internal PrimaryRegionStack(Construct scope, string id, IStackProps props = null) : base(scope, id, props)
    {
        Bucket = new Bucket(this, "Bucket", new BucketProps
        {
            BucketName = PhysicalName.GENERATE_IF_NEEDED
        });
        Bucket.ApplyRemovalPolicy(RemovalPolicy.DESTROY);
    }
}
