namespace Discord_bot_app.MessageFactory;

abstract class MessageFactory
{
    public abstract IMessageParent FactoryMethod();

    public string SomeOperation()
    {
        var product = FactoryMethod();

        var result = "Creator: The same creator's code has just worked with " + product.Operation();

        return result;
    }
}