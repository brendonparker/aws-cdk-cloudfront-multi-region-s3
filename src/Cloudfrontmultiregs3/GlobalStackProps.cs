using Amazon.CDK;
using Amazon.CDK.AWS.S3;

namespace Cloudfrontmultiregs3;

class GlobalStackProps : StackProps
{
    public IBucket PrimaryBucket { get; set; }
    public string SecondayBucketArnParameterName { get; set; }
}
