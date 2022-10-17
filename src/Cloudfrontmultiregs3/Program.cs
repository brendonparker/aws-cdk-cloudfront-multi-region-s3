using Amazon.CDK;

namespace Cloudfrontmultiregs3;

sealed class Program
{
    static string ACCOUNT => System.Environment.GetEnvironmentVariable("CDK_DEFAULT_ACCOUNT");
    public static void Main(string[] args)
    {
        var primaryRegion = "us-east-1";
        var secondaryBucketArnParameterName = "seconday-BucketArn";
        var secondaryRegion = "us-east-2";
        var pathToAssets = "./public";

        var app = new App();
        var stackPrimary = new PrimaryRegionStack(app, "PrimaryRegionStack", new PrimaryRegionStackProps
        {
            Env = new Amazon.CDK.Environment
            {
                Account = ACCOUNT,
                Region = primaryRegion,
            }
        });

        var stackSecondary = new SecondaryRegionStack(app, "SecondaryRegionStack", new SecondaryRegionStackProps
        {
            Env = new Amazon.CDK.Environment
            {
                Account = ACCOUNT,
                Region = secondaryRegion,
            },
            BucketArnParameterName = secondaryBucketArnParameterName
        });

        var stackGlobal = new GlobalStack(app, "GlobalStack", new GlobalStackProps
        {
            Env = new Amazon.CDK.Environment
            {
                Account = ACCOUNT,
                Region = primaryRegion,
            },
            PrimaryBucket = stackPrimary.Bucket,
            SecondayBucketArnParameterName = secondaryBucketArnParameterName,
            SecondaryRegion = secondaryRegion,
            PathToAssets = pathToAssets
        });

        stackGlobal.AddDependency(stackSecondary);

        app.Synth();
    }
}
