using Discord_bot_app.MessageFactory.Implementations;

namespace Discord_bot_app.MessageFactory.FactoryClasses;

class MessageClass1 : MessageFactory
{
        // Note that the signature of the method still uses the abstract product
        // type, even though the concrete product is actually returned from the
        // method. This way the Creator can stay independent of concrete product
        // classes.
        public override IMessageParent FactoryMethod()
        {
            return new MessageClass1Implement();
        }
}
