--统一的消息处理函数
msg,qq,group,id = nil,nil,nil,nil
local handled = false

--发送消息
--自动判断群聊与私聊
function sendMessage(s)
    if group then
        cqSendGroupMessage(group,s)
    else
        cqSendPrivateMessage(qq,s)
    end
end

--所有需要运行的app
local apps = {
    {--运行代码
        check = function ()
            return msg:find("#lua") == 1 or msg:find("--lua") == 1
        end,
        run = function ()
            local code
            if msg:find("#lua") == 1 then
                code = cqCqCode_UnTrope(msg:sub(5))
            elseif msg:find("--lua") == 1 then
                code = cqCqCode_UnTrope(msg:sub(6))
            end
            sendMessage(cqCode_At(qq).."\r\n"..apiSandBox(code))
            return true
        end,
    },
    {--查imei记录
        check = function ()
            return (msg:find("%[CQ:at,qq="..cqGetLoginQQ().."%]") or not group)
                and msg:find("%d") and msg:match("%d+"):len() == 15
        end,
        run = function ()
            local imei = msg:match("%d+")
            local html = apiHttpGet("http://erp.openluat.com/factory/v1/info?imei="..imei)
            if not html and html == "" then return end
            local d,r,e  = jsonDecode(html)
            if not r then return end
            if d["error"] == 0 then
                sendMessage(cqCode_At(qq).."\r\n"..
                "IMEI:"..d.imei.."\r\n"..
                "工单号:"..d.order_id.."\r\n"..
                "袋号:"..d.packed_packet_id.."\r\n"..
                "包装(真空袋):"..(d.packed_packet == 1 and "通过 "..d.packed_packet_time or "不通过").."\r\n"..
                "品质部门检测:"..(d.qc_pass == 1 and "通过 "..d.qc_time or "不通过").."\r\n"..
                "批量校准测试:"..(d.calib_pass == 1 and "通过 "..d.calib_time or "不通过").."\r\n"..
                "订制软件升级:"..(d.upgrade_pass == 1 and "通过 "..d.upgrade_time or "不通过").."\r\n"..
                "硬件功能检测:"..(d.ft_pass == 1 and "通过 "..d.ft_time or "不通过").."\r\n"..
                "写入IMEI串号:"..(d.write_imei_pass == 1 and "通过 "..d.write_imei_time or "不通过"))
            else
                sendMessage(cqCode_At(qq).."\r\n".."查询出错:"..d.message)
            end
            return true
        end,
    }
}

--对外提供的函数接口
return function (inmsg,inqq,ingroup,inid)
    msg,qq,group,id = inmsg,inqq,ingroup,inid

    --遍历所有功能
    for i=1,#apps do
        if apps[i].check and apps[i].check() then
            if apps[i].run() then
                handled = true
                break
            end
        end
    end
    return handled
end
