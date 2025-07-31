using System.Diagnostics;
using LotCom.Database.Models;
using LotCom.Services;
using LotCom.Types;

namespace LotCom.Database.Mappers;

public static class PrintMapper
{
    public static Print DtoToModel(PrintDto Dto)
    {
        // retrieve the Process and Part from the Database
        Process ModelProcess = ProcessService.Get(Dto.ProcessId);
        Part ModelPart = PartService.Get(Dto.PartId);
        // map any non-null variable fields
        VariableFieldSet ModelVariableFields = new VariableFieldSet();
        if (Dto.JBKNumber is not null)
        {
            ModelVariableFields.JBKNumber = new JBKNumber((int)Dto.JBKNumber);
        }
        if (Dto.LotNumber is not null)
        {
            ModelVariableFields.LotNumber = new LotNumber
            (
                int.Parse(Dto.LotNumber.Replace(" ", ""))
            );
        }
        if (Dto.DieNumber is not null)
        {
            ModelVariableFields.DieNumber = new DieNumber(Dto.DieNumber);
        }
        if (Dto.JBKNumber is not null)
        {
            ModelVariableFields.JBKNumber = new JBKNumber((int)Dto.JBKNumber);
        }
        if (Dto.JBKNumber is not null)
        {
            ModelVariableFields.JBKNumber = new JBKNumber((int)Dto.JBKNumber);
        }
        return new Print
            (
                ModelProcess,
                ModelPart,
                new VariableFieldSet
                (
                    Dto.JBKNumber,
                    Dto.LotNumber,
                    Dto.DieNumber,
                    Dto.DeburrJBKNumber,
                    Dto.HeatNumber
                )
            )
    }

    public static PrintDto ModelToDto(Print Model)
    {

    }
}