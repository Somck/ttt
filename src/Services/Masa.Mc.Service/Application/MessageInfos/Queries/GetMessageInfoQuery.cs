﻿// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

namespace Masa.Mc.Service.Admin.Application.MessageInfos.Queries;

public record GetMessageInfoQuery(Guid MessageInfoId) : Query<MessageInfoDto>
{
    public override MessageInfoDto Result { get; set; } = new();
}