﻿using MorseCode.ITask;

using Telegram.Bot.Interactions.Model.Responses.Implementation.Types;
using Telegram.Bot.Types;

namespace Telegram.Bot.Interactions.Tests.Environment.Parsers.Generic;

public class ValidGenericParser : IGenericParserBase<TextResponse>
{
    public bool CanParse(Message telegramMessage) { throw new NotImplementedException(); }

    public ITask<TextResponse> ParseResponseAsync(Message telegramMessage) { throw new NotImplementedException(); }
}