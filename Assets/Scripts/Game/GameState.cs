using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using Lua2CS;
using UnityEngine;
using UnityEngine.Networking;

public class GameState
{
    private IntPtr luaState;
    private int errorFuncRef;
    public delegate int LuaFunction(IntPtr luastate);

    // Start is called before the first frame update
    public GameState()
    {
        luaState = LuaAPI.luaL_newstate();
        LuaAPI.luaL_openlibs(luaState);
        // Register error function..
        var fn = Marshal.GetFunctionPointerForDelegate(new LuaFunction(SetTracebackMessage));
        LuaAPI.lua_pushcfunction(luaState, fn);
        errorFuncRef = LuaAPI.luaL_ref(luaState, LuaAPI.LUA_REGISTRYINDEX);
        // Register print function..
        fn = Marshal.GetFunctionPointerForDelegate(new LuaFunction(Print));
        LuaAPI.lua_register(luaState, "print", fn);

        fn = Marshal.GetFunctionPointerForDelegate(new LuaFunction(LoadFile));
        LuaAPI.lua_pushcfunction(luaState, fn);
    }

    private int LoadFile(IntPtr luastate)
    {
        var ptr = LuaAPI.lua_tolstring(luastate, 1, out IntPtr lenPtr);
        var fileName = LuaConverter.ToString(ptr, lenPtr);
        var bytes = LoadBytes(fileName);
        if (bytes != null)
        {
            if (LuaAPI.luaL_loadbuffer(luastate, bytes, bytes.Length, fileName) == 0)
            {
                LuaAPI.lua_pushboolean(luastate, 1);
                LuaAPI.lua_insert(luastate, -2);
                return 2;
            }
            else
            {
                throw new Exception("");
            }
        }
        else
        {
            throw new Exception("");
        }
    }

    private byte[] LoadBytes(string fileName)
    {
        var path = Path.Combine(Application.streamingAssetsPath, fileName);
        return File.ReadAllBytes(path);
    }

    private int Print(IntPtr luaState)
    {
        var n = LuaAPI.lua_gettop(luaState);
        LuaAPI.lua_getglobal(luaState, "tostring");
        var sb = new StringBuilder();
        for (int i = 1; i <= n; i++)
        {
            LuaAPI.lua_pushvalue(luaState, -1);
            LuaAPI.lua_pushvalue(luaState, i);
            LuaAPI.lua_call(luaState, 1, 1);
            var ptr = LuaAPI.lua_tolstring(luaState, -1, out IntPtr lenPtr);
            var str = LuaConverter.ToString(ptr, lenPtr);
            sb.Append(str);
            sb.Append("\t");
            LuaAPI.lua_pop(luaState, 1);
        }
        LuaAPI.lua_pop(luaState, n + 1);
        // System.Console.WriteLine(sb.ToString());
        Debug.Log(sb.ToString());
        return 0;
    }

    private int SetTracebackMessage(IntPtr luastate)
    {
        LuaAPI.lua_getglobal(luaState, "debug");
        LuaAPI.lua_getfield(luaState, -1, "traceback");
        LuaAPI.lua_remove(luaState, -2);
        LuaAPI.lua_pushvalue(luaState, -2);
        LuaAPI.lua_pushnumber(luaState, 2);
        LuaAPI.lua_call(luaState, 2, 1);
        LuaAPI.lua_remove(luaState, -2);
        return 1;
    }

    private void ThrowTopOfStackException(IntPtr luaState)
    {
        var ptr = LuaAPI.lua_tolstring(luaState, -1, out IntPtr lengthPtr);
        var message = LuaConverter.ToString(ptr, lengthPtr);
        LuaAPI.lua_pop(luaState, 1);
        throw new Exception(message);
    }

    public void ExecuteWithNoParamAndNoReturnValue(string content)
    {
        var chunkName = "Chunk";
        var bytes = Encoding.ASCII.GetBytes(content);
        LuaAPI.lua_rawgeti(luaState, LuaAPI.LUA_REGISTRYINDEX, errorFuncRef);
        var errorFunctionIndex = LuaAPI.lua_gettop(luaState);
        if (LuaAPI.luaL_loadbuffer(luaState, bytes, bytes.Length, chunkName) == 0)
        {
            if (LuaAPI.lua_pcall(luaState, 0, 0, errorFunctionIndex) == 0)
            {
                LuaAPI.lua_pop(luaState, 1);
            }
            else
            {
                ThrowTopOfStackException(luaState);
            }
        }
        else
        {
            ThrowTopOfStackException(luaState);
        }
    }
}
