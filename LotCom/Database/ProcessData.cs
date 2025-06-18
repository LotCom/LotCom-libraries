using LotCom.Enums;
using LotCom.Extensions;
using LotCom.Types;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LotCom.Database;

/// <summary>
/// Provides controlled access to Process data sources.
/// </summary>
public class ProcessData() 
{
    private const string Path = "\\\\144.133.122.1\\Lot Control Management\\Database\\process_control\\process_data.json";
        
    /// <summary>
    /// Contains the List of Processes produced by the last LoadData/LoadDataAsync call. 
    /// </summary>
    private List<Process>? CachedProcesses;

    private bool AreProcessesLoaded => (CachedProcesses is not null) && (CachedProcesses.Count > 0);

    /// <summary>
    /// Synchronously loads the data from the Process Masterlist data source. Stores this data in the LastRead property.
    /// </summary>
    /// <returns>A JSON dictionary containing the Process Masterlist data.</returns>
    /// <exception cref="SystemException"></exception>
    private static JObject LoadData()
    {
        // read the masterlist file
        try
        {
            return JObject.Parse(File.ReadAllText(Path));
        }
        catch (Exception _ex)
        {
            throw new SystemException
            (
                "Failed to read Process Data due to the following exception:"
                + $"\n\t{_ex.GetType()}"
                + $"\n\t{_ex.Message}"
            );
        }
    }

    /// <summary>
    /// Asynchronously loads the data from the Process Masterlist data source. Stores this data in the LastRead property.
    /// </summary>
    /// <exception cref="JsonException"></exception>
    private static async Task<JObject> LoadDataAsync()
    {
        // read the masterlist file
        try
        {
            return JObject.Parse(await File.ReadAllTextAsync(Path));
        }
        catch (Exception _ex)
        {
            throw new SystemException
            (
                "Failed to read Process Data due to the following exception:"
                + $"\n\t{_ex.GetType()}"
                + $"\n\t{_ex.Message}"
            );
        }
    }

    /// <summary>
    /// Attempts to resolve a Part object from the data in Token.
    /// </summary>
    /// <param name="Token">A JToken object containing Part data.</param>
    /// <param name="ParentProcess">The known Process that the Part should belong to.</param>
    /// <returns>A Part object with data resolved from the JToken.</returns>
    /// <exception cref="FormatException"></exception>
    private Part ResolvePartFromToken(JToken Token, string ParentProcess) 
    {
        // hold variables for each Part object property
        string Number;
        string Name;
        ModelNumber Model;
        // attempt to pull the needed fields from the passed JToken
        try 
        {
            Number = Token["Number"]!.ToString();
            Name = Token["Name"]!.ToString();
            Model = new ModelNumber(Token["Model"]!.ToString());
        // one of the needed fields was not accessible
        } 
        catch 
        {
            throw new FormatException($"Could not resolve '{Token}' to a Part object.");
        }
        // attempt to construct the Part object from the resolved data
        Part ResolvedPart;
        try 
        {
            ResolvedPart = new Part(ParentProcess, Number, Name, Model);
        } 
        catch 
        {
            throw new FormatException($"Could not resolve '{Token}' to a Part object.");
        }
        // return the resolved Part object
        return ResolvedPart;
    }

    /// <summary>
    /// Attempts to resolve a Process object from the data in Token.
    /// </summary>
    /// <param name="Token">A JToken object containing Part data.</param>
    /// <returns>A Process object with data resolved from the JToken.</returns>
    /// <exception cref="FormatException"></exception>
    private Process ResolveProcessFromToken(JToken Token) 
    {
        // hold variables for each Process object property
        int LineCode;
        string Line;
        string Title;
        OriginationType Type;
        SerializationMode Mode;
        JToken RawParts;
        JToken RawRequirements;
        PassThroughType PassThroughType;
        JToken RawPreviousProcesses;
        // attempt to access each field of Data from the Process Token
        try
        {
            LineCode = int.Parse(Token["LineCode"]!.ToString());
            Line = Token["Line"]!.ToString();
            Title = Token["Title"]!.ToString();
            Type = OriginationTypeExtensions.FromString(Token["Type"]!.ToString());
            Mode = SerializationModeExtensions.FromString(Token["Serialization"]!.ToString());
            RawParts = Token["Parts"]!;
            RawRequirements = Token["Requirements"]!;
            PassThroughType = PassThroughTypeExtensions.FromString(Token["PassThroughHeadingType"]!.ToString());
            RawPreviousProcesses = Token["PreviousProcesses"]!;
            // one of the needed fields was not accessible
        }
        catch
        {
            throw new FormatException($"Could not resolve '{Token}' to a Process object.");
        }
        // process and add each part to the parts list individually
        List<Part> Parts = [];
        string ProcessName = $"{LineCode}-{Line}-{Title}";
        try 
        {
            Parts = RawParts
                .Select(x =>
                ResolvePartFromToken(x, ProcessName))
                .ToList();
        // one of the Tokens could not be resolved to a Part
        } 
        catch (Exception _ex) 
        {
            throw new FormatException($"Could not resolve '{Token}' to a Process object, due to the following Part resolution failure: {_ex.Message}");
        }
        // create a RequiredFields object to parse the Process requirements into
        RequiredFields Requirements;
        try
        {
            Requirements = RequiredFields.ParseJSON(RawRequirements.ToString());
        }
        catch
        {
            throw new FormatException($"Could not resolve '{Token}' to a RequiredFields object.");
        }
        // attempt to retrieve all of the Process full names from PreviousProcesses to validate
        List<string?> PreviousProcesses = [];
        try
        {
            foreach (JToken _process in RawPreviousProcesses)
            {
                if (_process is not null && !_process.ToString().Equals("null"))
                {
                    PreviousProcesses.Add(_process.ToString());
                }
            }
        }
        catch (SystemException)
        {
            throw new FormatException("Could not resolve Process' PreviousProcesses property.");
        }
        // attempt to construct the Process object from the resolved data
        Process ResolvedProcess;
        try 
        {
            ResolvedProcess = new Process
            (
                LineCode,
                Line,
                Title,
                Type,
                Mode,
                Parts,
                Requirements,
                PassThroughType,
                PreviousProcesses
            );
        } 
        catch 
        {
            throw new FormatException($"Could not resolve '{Token}' to a Process object.");
        }
        // return the resolved Process object
        return ResolvedProcess;
    }

    /// <summary>
    /// Attempts to resolve a Department object from the data in Token.
    /// </summary>
    /// <param name="Token">A JToken object containing Department data.</param>
    /// <returns>A Department object with data resolved from the JToken.</returns>
    /// <exception cref="FormatException"></exception>
    private static Department ResolveDepartmentFromToken(JToken Token) 
    {
        // hold variables for each Department object property
        string Title;
        string Code;
        List<string> Lines = [];
        // attempt to access each field of Data from the Department Token
        try 
        {
            Title = Token["Title"]!.ToString();
            Code = Token["Code"]!.ToString();
        // one of the needed fields was not accessible
        } 
        catch 
        {
            throw new FormatException($"Could not resolve '{Token}' to a Department object.");
        }
        // add each Line to the Lines list individually
        try 
        {
            // resolve a Line from each Token
            Lines = Token["Lines"]!
                .Select(x => x
                .ToString())
                .ToList();
        // one of the Tokens could not be resolved to a Line
        } 
        catch (Exception _ex) 
        {
            throw new FormatException($"Could not resolve '{Token}' to a Department object, due to the following Line resolution failure: {_ex.Message}");
        }
        // attempt to construct the Department object from the resolved data
        Department ResolvedDepartment;
        try 
        {
            ResolvedDepartment = new Department(Title, Code, Lines);
        } 
        catch 
        {
            throw new FormatException($"Could not resolve '{Token}' to a Department object.");
        }
        // return the resolved Process object
        return ResolvedDepartment;
    }

    /// <summary>
    /// Retrieves the list of Processes, as Process objects, from the Process Masterlist.
    /// Stores this list in the CachedProcesses property.
    /// </summary>
    /// <returns>A list of Process objects.</returns>
    /// <exception cref="SystemException"></exception>
    /// <exception cref="JsonException"></exception>
    /// <exception cref="FormatException"></exception>
    public List<Process> GetAllProcesses() 
    {
        if (!AreProcessesLoaded) 
        {
            // load the data from the Masterlist
            JObject NewRead;
            try
            {
                NewRead = LoadData();
            }
            catch (SystemException _ex)
            {
                throw new SystemException
                (
                    "Failed to get Processes due to the following exception:"
                    + $"\n\t{_ex.GetType()}"
                    + $"\n\t{_ex.Message}"
                );
            }
            // get the list of Processes in the Masterlist
            if (NewRead!["Processes"] is null) 
            {
                throw new JsonException("Could not find Processes in the data from Process Data source.");
            }
            // convert the Process tokens into Process objects
            List<Process> ProcessObjects;
            try
            {
                ProcessObjects = NewRead["Processes"]!
                    .Select(ResolveProcessFromToken)
                    .ToList();
            }
            catch (FormatException _ex)
            {
                throw new FormatException
                (
                    "JSON stream contains invalid formatting that caused the following exception: "
                    + $"\n\t{_ex.GetType()}"
                    + $"\n\t{_ex.Message}"
                );
            }
            CachedProcesses = ProcessObjects;
        }
        return CachedProcesses!;
    }

    /// <summary>
    /// Asynchronously retrieves the list of Processes, as Process objects, from the Process Masterlist.
    /// Stores this list in the CachedProcesses property.
    /// </summary>
    /// <returns>A list of Process objects.</returns>
    /// <exception cref="SystemException"></exception>
    /// <exception cref="JsonException"></exception>
    /// <exception cref="FormatException"></exception>
    public async Task<List<Process>> GetAllProcessesAsync() 
    {
        if (!AreProcessesLoaded) 
        {
            CachedProcesses = await Task.Run(async () => 
            {
                // load the data from the Masterlist
                JObject NewRead;
                try
                {
                    NewRead = await LoadDataAsync();
                }
                catch (SystemException _ex)
                {
                    throw new SystemException
                    (
                        "Failed to get Processes due to the following exception:"
                        + $"\n\t{_ex.GetType()}"
                        + $"\n\t{_ex.Message}"
                    );
                }
                // return the list of Processes in the Masterlist
                if (NewRead!["Processes"] is null) 
                {
                    throw new JsonException("Could not find Processes in the data from Process Data source.");
                }
                // convert the Process tokens into Process objects
                List<Process> ProcessObjects;
                try
                {
                    ProcessObjects = NewRead["Processes"]!
                        .Select(ResolveProcessFromToken)
                        .ToList();
                }
                catch (FormatException _ex)
                {
                    throw new FormatException
                    (
                        "JSON stream contains invalid formatting that caused the following exception: "
                        + $"\n\t{_ex.GetType()}"
                        + $"\n\t{_ex.Message}"
                    );
                }
                return ProcessObjects;
            });
        }
        return CachedProcesses!;
    }

    /// <summary>
    /// Loads and queries the Process Masterlist data for data connected to ProcessFullName. 
    /// Returns a Process object constructed from the first found match.
    /// </summary>
    /// <param name="ProcessFullName">The FULL name of a Process to query for.</param>
    /// <returns>A Process object.</returns>
    /// <exception cref="FormatException"></exception>
    /// <exception cref="SystemException"></exception>
    /// <exception cref="ArgumentException"></exception>
    public Process GetIndividualProcess(string ProcessFullName)
    {
        try
        {
            GetAllProcesses();
        }
        catch (FormatException)
        {
            throw;
        }
        catch (Exception _ex)
        {
            throw new SystemException
            (
                $"An exception of type '{_ex.GetType()}' occurred while reading or processing Process Data: {_ex.Message}"
            );
        }
        // attempt to access the data for the passed Process
        if (CachedProcesses is not null && CachedProcesses.Count > 0)
        {
            try
            {
                return CachedProcesses
                    .Where(x => x.FullName == ProcessFullName)
                    .First();
            // no processes matched the name
            }
            catch (ArgumentNullException)
            {
                throw new ArgumentException
                (
                    $"Could not resolve process '{ProcessFullName}'.",
                    nameof(ProcessFullName)
                );
            }
        }
        // there were no processes loaded from the database
        else
        {
            throw new ArgumentException
            (
                $"Could not resolve process '{ProcessFullName}'.",
                nameof(ProcessFullName)
            );
        }
    }

    /// <summary>
    /// Asynchronously loads and queries the Process Masterlist data for data connected to ProcessFullName. 
    /// Returns a Process object constructed from the first found match.
    /// </summary>
    /// <param name="ProcessFullName">The FULL name of a Process to query for.</param>
    /// <returns>A Process object.</returns>
    /// <exception cref="FormatException"></exception>
    /// <exception cref="SystemException"></exception>
    /// <exception cref="ArgumentException"></exception>
    public async Task<Process> GetIndividualProcessAsync(string ProcessFullName) 
    {
        try
        {
            await GetAllProcessesAsync();
        }
        catch (FormatException)
        {
            throw;
        }
        catch (Exception _ex)
        {
            throw new SystemException
            (
                $"An exception of type '{_ex.GetType()}' occurred while reading or processing Process Data: {_ex.Message}"
            );
        }
        // attempt to access the data for the passed Process
        if (CachedProcesses is not null && CachedProcesses.Count > 0)
        {
            try
            {
                return CachedProcesses
                    .Where(x => x.FullName == ProcessFullName)
                    .First();
            // no processes matched the name
            }
            catch (ArgumentNullException)
            {
                throw new ArgumentException
                (
                    $"Could not resolve process '{ProcessFullName}'.",
                    nameof(ProcessFullName)
                );
            }
        }
        // there were no processes loaded from the database
        else
        {
            throw new ArgumentException
            (
                $"Could not resolve process '{ProcessFullName}'.",
                nameof(ProcessFullName)
            );
        }
    }

    /// <summary>
    /// Retrieves the Process Part list for the specified Process.
    /// </summary>
    /// <param name="ProcessFullName"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="SystemException"></exception>
    /// <exception cref="NullReferenceException"></exception>
    public List<Part> GetProcessParts(string ProcessFullName) 
    {
        // load the Process' data
        Process Process;
        try
        {
            Process = GetIndividualProcess(ProcessFullName);
        }
        catch (ArgumentException)
        {
            throw new ArgumentException($"Process '{ProcessFullName}' is not defined.", nameof(ProcessFullName));
        }
        catch (SystemException)
        {
            throw;
        }
        // no Part data was read
        if (Process.Parts.Count < 1)
        {
            throw new NullReferenceException($"No Parts defined for Process '{ProcessFullName}'.");
        } 
        // return the Part list
        return Process.Parts;
    }

    /// <summary>
    /// Asynchronously retrieves the Process Part list for the specified Process.
    /// </summary>
    /// <param name="ProcessFullName">Process FULL Name ("Code-Title") to retrieve Part Data for.</param>
    /// <returns>A list of Part objects assigned to the Process.</returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="SystemException"></exception>
    /// <exception cref="NullReferenceException"></exception>
    public async Task<List<Part>> GetProcessPartsAsync(string ProcessFullName) 
    {
        // load the Process' data
        Process Process;
        try
        {
            Process = await GetIndividualProcessAsync(ProcessFullName);
        }
        catch (ArgumentException)
        {
            throw new ArgumentException($"Process '{ProcessFullName}' is not defined.", nameof(ProcessFullName));
        }
        catch (SystemException)
        {
            throw;
        }
        // no Part data was read
        if (Process.Parts.Count < 1)
        {
            throw new NullReferenceException($"No Parts defined for Process '{ProcessFullName}'.");
        } 
        // return the Part list
        return Process.Parts;
    }

    /// <summary>
    /// Queries for a Part matching PartNumber in ProcessFullName's Part data.
    /// </summary>
    /// <param name="ProcessFullName">The FULL Name ("Code-Title") of the Process to query from.</param>
    /// <param name="PartNumber">The Part Number to query for within ProcessFullName's data.</param>
    /// <returns>A JToken object containing the Part data for PartNumber.</returns>
    /// <exception cref="SystemException"></exception>
    /// <exception cref="ArgumentException"></exception>
    public Part GetProcessPartData(string ProcessFullName, string PartNumber) 
    {
        // retrieve the Process' Part list
        List<Part> ProcessParts;
        try
        {
            ProcessParts = GetProcessParts(ProcessFullName);
        }
        catch (SystemException)
        {
            throw;
        }
        // attempt to access the specific Part
        Part? SelectedPart;
        try 
        {
            SelectedPart = ProcessParts
                .Where(x => x.PartNumber
                .Equals(PartNumber))
                .First();
        // Part was not found in the Process' Part list
        } 
        catch (ArgumentNullException)
        {
            throw new ArgumentException($"Part '{PartNumber}' not defined for Process '{ProcessFullName}'.", nameof(PartNumber));
        }
        return SelectedPart;
    }

    /// <summary>
    /// Asynchronously queries for a Part matching PartNumber in ProcessFullName's Part data.
    /// </summary>
    /// <param name="ProcessFullName">The FULL Name ("Code-Title") of the Process to query from.</param>
    /// <param name="PartNumber">The Part Number to query for within ProcessFullName's data.</param>
    /// <returns>A JToken object containing the Part data for PartNumber.</returns>
    /// <exception cref="SystemException"></exception>
    /// <exception cref="ArgumentException"></exception>
    public async Task<Part> GetProcessPartDataAsync(string ProcessFullName, string PartNumber) 
    {
        // perform the query on a new CPU thread
        Part PartData = await Task.Run(async () => 
        {
            // retrieve the Process' Part list
            List<Part> ProcessParts;
            try
            {
                ProcessParts = await GetProcessPartsAsync(ProcessFullName);
            }
            catch (SystemException)
            {
                throw;
            }
            // attempt to access the specific Part
            Part? SelectedPart;
            try 
            {
                SelectedPart = ProcessParts
                    .Where(x => x.PartNumber
                    .Equals(PartNumber))
                    .First();
            // Part was not found in the Process' Part list
            } 
            catch (ArgumentNullException)
            {
                throw new ArgumentException($"Part '{PartNumber}' not defined for Process '{ProcessFullName}'.", nameof(PartNumber));
            }
            return SelectedPart;
        });
        // return the queried Part data
        return PartData;
    }
}