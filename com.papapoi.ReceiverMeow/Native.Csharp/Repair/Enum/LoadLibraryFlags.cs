﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Native.Csharp.Repair.Enum
{
	/*
	 *	移植自: 00.00.dotnetRedirect 插件, 原作者: 成音S. 引用请带上此注释
	 *	论坛地址: https://cqp.cc/t/42920
	 */
	[System.Flags]
	public enum LoadLibraryFlags : uint
	{
		None = 0,
		DONT_RESOLVE_DLL_REFERENCES = 0x00000001,
		LOAD_IGNORE_CODE_AUTHZ_LEVEL = 0x00000010,
		LOAD_LIBRARY_AS_DATAFILE = 0x00000002,
		LOAD_LIBRARY_AS_DATAFILE_EXCLUSIVE = 0x00000040,
		LOAD_LIBRARY_AS_IMAGE_RESOURCE = 0x00000020,
		LOAD_LIBRARY_SEARCH_APPLICATION_DIR = 0x00000200,
		LOAD_LIBRARY_SEARCH_DEFAULT_DIRS = 0x00001000,
		LOAD_LIBRARY_SEARCH_DLL_LOAD_DIR = 0x00000100,
		LOAD_LIBRARY_SEARCH_SYSTEM32 = 0x00000800,
		LOAD_LIBRARY_SEARCH_USER_DIRS = 0x00000400,
		LOAD_WITH_ALTERED_SEARCH_PATH = 0x00000008
	}
}
