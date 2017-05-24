(*
    05/23/2017 - KC
    F# library for generating sql scripts for Autopopulate.
    Please feel free to improve/clean up this code.
    The code is ugly due to it being created with no prior experience in F# 
*)

namespace AutoPopSqlGeneratorFSharp

open System

module SqlGenerators =

    let ternary(s:string) =
        if (s = null)
        then "NULL"
        else
            String.Format("\'{0}\'", s)

    // for SQL
    let GenerateNodePathSql (path:string) =
        let nodes = path.Replace(" ", "").Split[|'>'|]
        let nodeLevels = nodes.Length
        let mustNotExist = 
            "MUST_EXIST=REV_AUTO_POPULATE" + System.Environment.NewLine + System.Environment.NewLine

        let insert = 
            "insert into REV_AUTO_POPULATE (REV_AUTO_POPULATE_GU,NAME,QUESTION,PARENT_AUTO_GU)" + System.Environment.NewLine

        let select (input1:string, input2:string, input3:string) = 
            fun x -> x + String.Format("select newid(), \'{0}\', \'{1}\', {2}", input1, input2, input3) + System.Environment.NewLine
    
        let from = 
            fun x -> x + "from REV_AUTO_POPULATE a1" + System.Environment.NewLine

        let rec buildInnerJoin (level:int) =
            if (level < 3) then ""
            else 
                buildInnerJoin(level - 1)
                + String.Format("inner join REV_AUTO_POPULATE a{0} on (a{1}.PARENT_AUTO_GU = a{2}.REV_AUTO_POPULATE_GU)", level - 1, level - 1, level - 2) 
                + System.Environment.NewLine

        let innerJoin (level:int) =
            fun x -> x + buildInnerJoin(level)
    
        let rec buildWhereAnd (level:int) =
            if (level < 3) then ""
            else
                buildWhereAnd(level - 1)
                + String.Format(" and a{0}.Name = \'{1}\'", level - 1, nodes.[level-2]) 

        let buildWhere (level:int)=
            match level with
            | 1 -> String.Format("where \'{0}\' not in ", nodes.[0])
            | 2 -> String.Format("where a1.Name = \'{0}\' and a1.PARENT_AUTO_GU is null ", nodes.[0])
            | _ -> String.Format("where a1.Name = \'{0}\' and a1.PARENT_AUTO_GU is null", nodes.[0]) + buildWhereAnd(level)

        let where (level:int) =
            fun x -> x + buildWhere(level)

        let addNewLine =
            fun x -> x + System.Environment.NewLine

        let buildNotExists (level:int) =
            if (level = 1)
            then fun x -> x + "(select NAME from REV_AUTO_POPULATE where PARENT_AUTO_GU is null)$$$"
            else 
                fun x -> x 
                        + " and not exists "
                        + System.Environment.NewLine
                        + String.Format("(select * from REV_AUTO_POPULATE b1 where b1.PARENT_AUTO_GU = a{0}.REV_AUTO_POPULATE_GU and b1.NAME = \'{1}\')$$$", level-1, nodes.[level-1]) 

        let level1Script =
            insert |> select(nodes.[0], nodes.[0], "null") |> where(1) |> buildNotExists(1) |> addNewLine |> addNewLine

        let level2Script =
            insert |> select(nodes.[1], nodes.[1], "a1.REV_AUTO_POPULATE_GU") |> from |> where(2) |> buildNotExists(2) |> addNewLine |> addNewLine

        let levelNScript (level:int) = 
            insert |> select(nodes.[level-1], nodes.[level-1], String.Format("a{0}.REV_AUTO_POPULATE_GU", level-1)) |> from |> innerJoin(level) |> where(level) |> buildNotExists(level) |> addNewLine |> addNewLine
               
        let rec buildPath (level:int) =
            if (level = 1) 
            then level1Script
            else if (level = 2) 
            then buildPath(level-1) + level2Script
            else
                buildPath(level-1) + levelNScript level

        (mustNotExist + buildPath nodeLevels)

    // for Sql Autopop Items
    let GenerateResponseInsertSql (path:string, listOfResponses:string[] list) =

        let nodes = path.Replace(" ", "").Split[|'>'|]

        let nodeLevels = nodes.Length

        let responses = listOfResponses

        let insert = 
            "insert into REV_AUTO_POPULATE_RESPONSE (REV_AUTO_POPULATE_RESPONSE_GU, REV_AUTO_POPULATE_GU, NAME, RESPONSE, ADDL_RESPONSE)" + System.Environment.NewLine
        
        let select name response addl_response =
            fun x -> x + String.Format("select newid(), a{0}.REV_AUTO_POPULATE_GU, {1}, {2}, {3}", nodeLevels, ternary name, ternary response, ternary addl_response)

        let from =
            fun x -> x + " from REV_AUTO_POPULATE a1" + System.Environment.NewLine

        let rec buildInnerJoin (level:int) =
            if (level = 1) then ""
            else 
                buildInnerJoin(level - 1)
                + String.Format("inner join REV_AUTO_POPULATE a{0} on (a{1}.PARENT_AUTO_GU = a{2}.REV_AUTO_POPULATE_GU)", level, level, level - 1) 
                + System.Environment.NewLine

        let innerJoin (level:int) =
            fun x -> x + buildInnerJoin(level)

        let rec buildWhereAnd (level:int) =
            if (level = 1) then String.Format("where a1.NAME=\'{0}\' and a1.PARENT_AUTO_GU is null and ", nodes.[0])
            else
                buildWhereAnd(level - 1)
                + String.Format("a{0}.NAME = \'{1}\' and ", level, nodes.[level-1])

        let whereAnd (level:int) =
            fun x -> x + buildWhereAnd(level) + System.Environment.NewLine

        let addNewLine =
            fun x -> x + System.Environment.NewLine

        let notExists name =
            fun x -> x + String.Format("not exists (select * from REV_AUTO_POPULATE_RESPONSE arp where arp.REV_AUTO_POPULATE_GU = a{0}.REV_AUTO_POPULATE_GU and arp.NAME = \'{1}\')$$$", nodeLevels, name)

        let result = 
            let mutable res = ""
            for i in responses do
                res <- res + insert |> select i.[0] i.[1] i.[2] |> from |> innerJoin(nodeLevels) |> whereAnd(nodeLevels) |> notExists i.[0] |> addNewLine |> addNewLine
            (res)
    
        (result)

    // for Oracle
    let GenerateNodePathOracle (path:string) =
        let nodes = path.Replace(" ", "").Split[|'>'|]

        let nodeLevels = nodes.Length

        let mustNotExist = 
            "MUST_EXIST=REV_AUTO_POPULATE" + System.Environment.NewLine + System.Environment.NewLine

        let insert = 
            "insert into REV_AUTO_POPULATE (REV_AUTO_POPULATE_GU,NAME,QUESTION,PARENT_AUTO_GU)" + System.Environment.NewLine

        let select (input1:string, input2:string, input3:string) = 
            fun x -> x + String.Format("select sys_guid(), \'{0}\', \'{1}\', {2}", input1, input2, input3) + System.Environment.NewLine
    
        let fromDual =
            fun x -> x + "from dual" + System.Environment.NewLine

        let from = 
            fun x -> x + "from REV_AUTO_POPULATE a1" + System.Environment.NewLine

        let rec buildInnerJoin (level:int) =
            if (level < 3) then ""
            else 
                buildInnerJoin(level - 1)
                + String.Format("inner join REV_AUTO_POPULATE a{0} on (a{1}.PARENT_AUTO_GU = a{2}.REV_AUTO_POPULATE_GU)", level - 1, level - 1, level - 2) 
                + System.Environment.NewLine

        let innerJoin (level:int) =
            fun x -> x + buildInnerJoin(level)
    
        let rec buildWhereAnd (level:int) =
            if (level < 3) then ""
            else
                buildWhereAnd(level - 1)
                + String.Format(" and a{0}.Name = \'{1}\'", level - 1, nodes.[level-2]) 

        let buildWhere (level:int)=
            match level with
            | 1 -> String.Format("where \'{0}\' not in ", nodes.[0])
            | 2 -> String.Format("where a1.Name = \'{0}\' and a1.PARENT_AUTO_GU is null ", nodes.[0])
            | _ -> String.Format("where a1.Name = \'{0}\' and a1.PARENT_AUTO_GU is null", nodes.[0]) + buildWhereAnd(level)

        let where (level:int) =
            fun x -> x + buildWhere(level)

        let addNewLine =
            fun x -> x + System.Environment.NewLine

        let buildNotExists (level:int) =
            if (level = 1)
            then fun x -> x + "(select NAME from REV_AUTO_POPULATE where PARENT_AUTO_GU is null)$$$"
            else 
                fun x -> x 
                        + " and not exists "
                        + System.Environment.NewLine
                        + String.Format("(select * from REV_AUTO_POPULATE b1 where b1.PARENT_AUTO_GU = a{0}.REV_AUTO_POPULATE_GU and b1.NAME = \'{1}\')$$$", level-1, nodes.[level-1]) 

        let level1Script =
            insert |> select(nodes.[0], nodes.[0], "null") |> fromDual |> where(1) |> buildNotExists(1) |> addNewLine |> addNewLine

        let level2Script =
            insert |> select(nodes.[1], nodes.[1], "a1.REV_AUTO_POPULATE_GU") |> from |> where(2) |> buildNotExists(2) |> addNewLine |> addNewLine

        let levelNScript (level:int) = 
            insert |> select(nodes.[level-1], nodes.[level-1], String.Format("a{0}.REV_AUTO_POPULATE_GU", level-1)) |> from |> innerJoin(level) |> where(level) |> buildNotExists(level) |> addNewLine |> addNewLine

                 
        let rec result (level:int) =
            if (level = 1) 
            then level1Script
            else if (level = 2) 
            then result(level-1) + level2Script
            else
                result(level-1) + levelNScript level

        (mustNotExist + result nodeLevels)

    // for Oracle Autopop Items
    let GenerateResponseInsertOracle (path:string, listOfResponses:string[] list) =

        let nodes = path.Replace(" ", "").Split[|'>'|]

        let nodeLevels = nodes.Length

        let responses = listOfResponses

        let insert = 
            "insert into REV_AUTO_POPULATE_RESPONSE (REV_AUTO_POPULATE_RESPONSE_GU, REV_AUTO_POPULATE_GU, NAME, RESPONSE, ADDL_RESPONSE)" + System.Environment.NewLine
        
        let select name response addl_response =
            fun x -> x + String.Format("select sys_guid(), a{0}.REV_AUTO_POPULATE_GU, {1}, {2}, {3}", nodeLevels, ternary name, ternary response, ternary addl_response)

        let from =
            fun x -> x + " from REV_AUTO_POPULATE a1" + System.Environment.NewLine

        let rec buildInnerJoin (level:int) =
            if (level = 1) then ""
            else 
                buildInnerJoin(level - 1)
                + String.Format("inner join REV_AUTO_POPULATE a{0} on (a{1}.PARENT_AUTO_GU = a{2}.REV_AUTO_POPULATE_GU)", level, level, level - 1) 
                + System.Environment.NewLine

        let innerJoin (level:int) =
            fun x -> x + buildInnerJoin(level)

        let rec buildWhereAnd (level:int) =
            if (level = 1) then String.Format("where a1.NAME=\'{0}\' and a1.PARENT_AUTO_GU is null and ", nodes.[0])
            else
                buildWhereAnd(level - 1)
                + String.Format("a{0}.NAME = \'{1}\' and ", level, nodes.[level-1])

        let whereAnd (level:int) =
            fun x -> x + buildWhereAnd(level) + System.Environment.NewLine

        let addNewLine =
            fun x -> x + System.Environment.NewLine

        let notExists name =
            fun x -> x + String.Format("not exists (select * from REV_AUTO_POPULATE_RESPONSE arp where arp.REV_AUTO_POPULATE_GU = a{0}.REV_AUTO_POPULATE_GU and arp.NAME = \'{1}\')$$$", nodeLevels, name)

        let result = 
            let mutable res = ""
            for i in responses do
                res <- res + insert |> select i.[0] i.[1] i.[2] |> from |> innerJoin(nodeLevels) |> whereAnd(nodeLevels) |> notExists i.[0] |> addNewLine |> addNewLine
            (res)
    
        (result)