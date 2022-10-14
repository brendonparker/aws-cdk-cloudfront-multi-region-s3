using Amazon.CDK;
using System.Linq;

namespace Cloudfrontmultiregs3;

sealed class Program
{
    public static void Main(string[] args)
    {
        var secondaryBucketArnParameterName = "seconday-BucketArn";
        var app = new App();
        var stackPrimary = new PrimaryRegionStack(app, "PrimaryRegionStack", new StackProps
        {
            Env = new Amazon.CDK.Environment
            {
                Account = System.Environment.GetEnvironmentVariable("CDK_DEFAULT_ACCOUNT"),
                Region = "us-east-1",
            }
        });

        var stackSecondary = new SecondaryRegionStack(app, "SecondaryRegionStack", new SecondaryRegionStackProps
        {
            Env = new Amazon.CDK.Environment
            {
                Account = System.Environment.GetEnvironmentVariable("CDK_DEFAULT_ACCOUNT"),
                Region = "us-east-2",
            },
            BucketArnParameterName = secondaryBucketArnParameterName
        });

        var stackGlobal = new GlobalStack(app, "GlobalStack", new GlobalStackProps
        {
            Env = new Amazon.CDK.Environment
            {
                Account = System.Environment.GetEnvironmentVariable("CDK_DEFAULT_ACCOUNT"),
                Region = "us-east-1",
            },
            PrimaryBucket = stackPrimary.Bucket,
            SecondayBucketArnParameterName = secondaryBucketArnParameterName
        });

        stackGlobal.AddDependency(stackSecondary);

        app.Synth();
    }
}
