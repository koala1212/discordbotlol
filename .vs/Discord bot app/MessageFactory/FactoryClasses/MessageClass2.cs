using Discord_bot_app.MessageFactory.Implementations;

namespace Discord_bot_app.MessageFactory.FactoryClasses;


    class MessageClass2 : MessageFactory
    {
        public override IMessageParent FactoryMethod()
        {
            return new MessageClass2Implement();
        }
    }

