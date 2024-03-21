using Telegram.Bot.Interactions.Model.Descriptors;
using Telegram.Bot.Interactions.Model.Descriptors.Loading.Abstraction;
using Telegram.Bot.Interactions.Model.Responses.Abstraction;
using Telegram.Bot.Interactions.Model.Responses.Implementation.Types;
using Telegram.Bot.Interactions.Services;
using Telegram.Bot.Interactions.Tests.Environment.Responses;
using Telegram.Bot.Interactions.Tests.Environment.Validators;
using Telegram.Bot.Interactions.Tests.Environment.Validators.Configs;
using Telegram.Bot.Interactions.Validators.Configs;

namespace Telegram.Bot.Interactions.Tests.Loading;

[TestFixture]
[TestOf(typeof(EntitiesLoader))]
public class ValidatorTests : BaseLoadingTests
{
    private static readonly string[] _invalidValidatorNames = {
        nameof(InvalidConfigValidator),
        nameof(InvalidAbstractValidator),
    };
    
    private static readonly string[] _validValidatorNames = {
        nameof(ValidGenericValidator),
        nameof(ValidAcceptAnyValidator),
        nameof(ValidAcceptMultipleValidator),
    };
    
    [Test]
    public void TestLoading_NoSP_NoStrict()
    {
        InteractionService.Config.StrictLoadingModeEnabled = false;
        
        GenericMultipleLoadingResult<ResponseValidatorInfo> loadingResult = 
            InteractionService.Loader.LoadResponseValidators(EnvironmentAssembly);
        Assert.That(loadingResult.Loaded, Is.True);
        
        IEnumerable<string> failedParserNames = loadingResult.Entities!
            .Where(e => !e.Loaded)
            .Select(e => e.EntityName);
        
        IEnumerable<string> loadedParserNames = loadingResult.Entities!
             .Where(e => e.Loaded)
             .Select(e => e.EntityName);
        
        CollectionAssert.AreEquivalent(_invalidValidatorNames, failedParserNames);
        CollectionAssert.AreEquivalent(_validValidatorNames, loadedParserNames);

        // Registration test
        CollectionAssert.IsSubsetOf(_validValidatorNames,
            InteractionService.Registry.ResponseValidators.Values
                              .Select(v => v.ValidatorType.Name));

        ResponseValidatorInfo genericValidatorInfo = 
            InteractionService.Registry.ResponseValidators[typeof(ValidGenericValidator)];
        ResponseValidatorInfo acceptAnyValidatorInfo = 
            InteractionService.Registry.ResponseValidators[typeof(ValidAcceptAnyValidator)];
        ResponseValidatorInfo acceptMultipleValidatorInfo = 
            InteractionService.Registry.ResponseValidators[typeof(ValidAcceptMultipleValidator)];
        
        Assert.Multiple(() => {
            Assert.That(genericValidatorInfo.ServiceProvider, Is.Null);
            Assert.That(genericValidatorInfo.RegisteredInTheSP, Is.False);
            Assert.That(genericValidatorInfo.ResponseType.IsEquivalentTo(typeof(IAbstractResponse)));
            Assert.That(genericValidatorInfo.AvailableConfigTypes, Has.Count.EqualTo(1));
            Assert.That(genericValidatorInfo.AvailableConfigTypes[0]
                            .IsEquivalentTo(typeof(IResponseModelConfig<IAbstractResponse>)));
            
            Assert.That(acceptAnyValidatorInfo.ServiceProvider, Is.Null);
            Assert.That(acceptAnyValidatorInfo.RegisteredInTheSP, Is.False);
            Assert.That(acceptAnyValidatorInfo.ResponseType.IsEquivalentTo(typeof(TextResponse)));
            Assert.That(acceptAnyValidatorInfo.AvailableConfigTypes, Has.Count.EqualTo(1));
            Assert.That(acceptAnyValidatorInfo.AvailableConfigTypes[0]
                            .IsEquivalentTo(typeof(IResponseModelConfig<IUserResponse>)));
            
            Assert.That(acceptMultipleValidatorInfo.ServiceProvider, Is.Null);
            Assert.That(acceptMultipleValidatorInfo.RegisteredInTheSP, Is.False);
            Assert.That(acceptMultipleValidatorInfo.ResponseType.IsEquivalentTo(typeof(ImageResponse)));
            Assert.That(acceptMultipleValidatorInfo.AvailableConfigTypes, Has.Count.EqualTo(3));
            CollectionAssert.AreEquivalent(new[] {
                typeof(TestTextConfig),
                typeof(ImageTestConfig),
                typeof(AbstractConfigImpl),
            }, acceptMultipleValidatorInfo.AvailableConfigTypes);
        });

        GenericLoadingResult<ResponseValidatorInfo> notParserLoadingResult =
            InteractionService.Loader.LoadResponseValidator(typeof(AbstractConfigImpl));
        GenericLoadingResult<ResponseValidatorInfo> singleParserLoadingResult =
            InteractionService.Loader.LoadResponseValidator<TextResponse, ValidAcceptAnyValidator>();

        Assert.Multiple(() =>
        {
            Assert.That(notParserLoadingResult.Loaded, Is.False);
            Assert.That(singleParserLoadingResult.Loaded, Is.False);
        });
        ;
    }
}