using AutoMapper;

namespace TranslationManagement.Business.Tests;

public abstract class BllTests
{
    protected IMapper Mapper { get; private set; }

    [OneTimeSetUp]
    public virtual void OneTimeSetUp()
    {
        Mapper = new MapperConfiguration(cfg => cfg.ConfigureBusinessMappings()).CreateMapper();
    }
}