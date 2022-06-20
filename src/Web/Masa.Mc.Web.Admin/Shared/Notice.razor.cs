﻿// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

namespace Masa.Mc.Web.Admin.Shared;

public partial class Notice : AdminCompontentBase
{
    [Inject]
    public NoticeState NoticeState { get; set; } = default!;

    private GetWebsiteMessageInputDto _queryParam = new(5);
    private bool isHubConnectionBuilder { get; set; }

    WebsiteMessageService WebsiteMessageService => McCaller.WebsiteMessageService;

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        NoticeState.OnNoticeChanged += Changed;

        await WebsiteMessageService.CheckAsync();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender && !isHubConnectionBuilder)
        {
            isHubConnectionBuilder = true;
            await LoadData();

            await base.HubConnectionBuilder();

            base.HubConnection?.On(SignalRMethodConsts.GET_NOTIFICATION, async () =>
            {
                await LoadData();
            });

            base.HubConnection?.On(SignalRMethodConsts.CHECK_NOTIFICATION, async () =>
            {
                await WebsiteMessageService.CheckAsync();
            });
        }
        await base.OnAfterRenderAsync(firstRender);
    }

    async Task LoadData()
    {
        var dtos = await WebsiteMessageService.GetListAsync(_queryParam);
        NoticeState.SetNotices(dtos.Result);
    }

    async Task Changed()
    {
        await InvokeAsync(StateHasChanged);
    }

    public override void Dispose()
    {
        NoticeState.OnNoticeChanged -= Changed;
    }
}