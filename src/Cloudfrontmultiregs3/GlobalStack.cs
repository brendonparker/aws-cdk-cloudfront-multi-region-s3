using Amazon.CDK;
using CloudFront = Amazon.CDK.AWS.CloudFront;
using Constructs;
using S3 = Amazon.CDK.AWS.S3;

namespace Cloudfrontmultiregs3;

class GlobalStack : Stack
{
    internal GlobalStack(Construct scope, string id, GlobalStackProps props) : base(scope, id, props)
    {
        var secondaryBucketArn = new SSMParameterReader(this, "SecondaryBucketArn", new SSMParameterReader.SSMParameterReaderProps
        {
            ParameterName = props.SecondayBucketArnParameterName,
            Region = "us-east-1"
        });
        var secondaryBucket = S3.Bucket.FromBucketArn(this, "SecondaryBucket", secondaryBucketArn.GetParameterValue());

        new CloudFront.Distribution(this, "Distribution", new CloudFront.DistributionProps
        {
            DefaultBehavior = new CloudFront.BehaviorOptions
            {
                ViewerProtocolPolicy = CloudFront.ViewerProtocolPolicy.REDIRECT_TO_HTTPS,
                //Origin = new CloudFront.Origins.S3Origin(props.PrimaryBucket)
                Origin = new CloudFront.Origins.OriginGroup(new CloudFront.Origins.OriginGroupProps
                {
                    PrimaryOrigin = new CloudFront.Origins.S3Origin(props.PrimaryBucket),
                    FallbackOrigin = new CloudFront.Origins.S3Origin(secondaryBucket),
                    FallbackStatusCodes = new double[] { 404, 500, 502, 503, 504 }
                })
            },
            DefaultRootObject = "index.html",
            ErrorResponses = new[]
            {
                new CloudFront.ErrorResponse
                {
                    HttpStatus = 403,
                    ResponseHttpStatus = 200,
                    ResponsePagePath = "/index.html",
                    Ttl = Duration.Minutes(5)
                }
            },
            MinimumProtocolVersion = CloudFront.SecurityPolicyProtocol.TLS_V1_2_2021
        });
    }
}