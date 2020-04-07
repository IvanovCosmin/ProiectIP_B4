using System;
using System.Collections.Generic;
using System.Text;

namespace IP_Framework
{

    class EventHandlerContext  // the context needed for the invoke function
    {
        public EventHandlerFunctions command;
        public IContext contextHandler;
        public CoreKnownFunctions coreCommand;
        public SubModuleFunctions subModuleCommand;

        public EventHandlerContext()
        {
            command = EventHandlerFunctions.InvalidCommand;
            coreCommand = CoreKnownFunctions.InvalidCommand;
            subModuleCommand = SubModuleFunctions.InvalidCommand;
            contextHandler = null;
        }

        public EventHandlerContext(byte[] initContext, int initSizeOfContext) // this will be changed with an interface for contexts
        {
            command = EventHandlerFunctions.InvalidCommand;
            coreCommand = CoreKnownFunctions.InvalidCommand;
            subModuleCommand = SubModuleFunctions.InvalidCommand;
            contextHandler = new IContext(initContext, initSizeOfContext);
        }

    }

    class EventHandler
    {
        private SymptomBasedDetection SymptomBasedModule;
        private EpidemyAlert EpidemyModule;
        private ImageProcessing ImageModule;
        private SymptomLearning SymptomModule;
        //private DataBaseHandler dbHandler; // TODO after the DB is alive
        // all modules should pe private, we need to encapsulate as much as possible
        // only this instances should have access to the data in the handler

        public EventHandler() // this module should get an DB instance for the dataBaseHandler
        {
            Init(null, 0);
            Console.WriteLine("A mers initilizarea!");
        }

        ~EventHandler()
        {
            this.UnInit(); // this should be like this because we need the same code for forced de-initialization
            Console.WriteLine("A mers de-initilizarea!");

        }

        public bool InvokeCommand(EventHandlerContext handlerContext) // invoke commands between the submodules and also the Core
        {
            Console.WriteLine("InvokeCommand execution for subModule Handler");

            if (ValidateContext(handlerContext) == false)
                return false;
            switch (handlerContext.command)
            {
                case EventHandlerFunctions.Init:
                    return this.Init(handlerContext.contextHandler.context, handlerContext.contextHandler.sizeOfContext);
                case EventHandlerFunctions.UnInit:
                    return UnInit();
                case EventHandlerFunctions.RequestCommand:
                    return RequestCommand(handlerContext.coreCommand, handlerContext.contextHandler.context, handlerContext.contextHandler.sizeOfContext);
                case EventHandlerFunctions.EpidemyAlertModule:
                    return EpidemyModule.InvokeCommand(handlerContext.subModuleCommand, handlerContext.contextHandler);
                case EventHandlerFunctions.ImageProcessingModule:
                    return ImageModule.InvokeCommand(handlerContext.subModuleCommand, handlerContext.contextHandler);
                case EventHandlerFunctions.SymptomLearningModule:
                    return SymptomModule.InvokeCommand(handlerContext.subModuleCommand, handlerContext.contextHandler);
                case EventHandlerFunctions.SymptomBasedDetectionModule:
                    return SymptomBasedModule.InvokeCommand(handlerContext.subModuleCommand, handlerContext.contextHandler);
                case EventHandlerFunctions.InvokeCommand:
                    return false; // INvoKE COmMAnD
            }
            return false;
        }

        private bool RequestCommand(CoreKnownFunctions command, byte[] context, int sizeOfContext)  // Sends requests to Core and then process them as we want
        {
            // invoke commands directly from the core of the program itself
            // directly send command to core, where the context should be validated!!!!
            // we could return actually a command, not just asking, but its better like this because we can control the state of the module in case of malfunctioning after our call
            // also, if we return the command, we can't use the context asked
            return true;
        }

        private bool Init(byte[] context, int sizeOfContext)  // Initialize the data using the possible context, it should be checked if context is not null if its mandatory for Init
        {
            SymptomBasedModule = new SymptomBasedDetection(this);
            SymptomBasedModule.Init(context, sizeOfContext);
            EpidemyModule = new EpidemyAlert(this);
            EpidemyModule.Init(context, sizeOfContext);
            ImageModule = new ImageProcessing(this);
            ImageModule.Init(context, sizeOfContext);
            SymptomModule = new SymptomLearning(this);
            SymptomModule.Init(context, sizeOfContext);
            return true;
        }

        private bool UnInit()  // unInit all the modules and destroy all data left in memory
        {
            SymptomBasedModule.UnInit();
            EpidemyModule.UnInit();
            ImageModule.UnInit();
            SymptomModule.UnInit();
            return true;
        }

        private bool ValidateContext(EventHandlerContext contextContainer)
        {
            if (ReferenceEquals(contextContainer, null))  // useless until EventHandlerContext becomes a class, not a struct
                return false;
            if (contextContainer.contextHandler.sizeOfContext == 0)
                return false;
            if (ReferenceEquals(contextContainer.contextHandler.context, null))
                return false;
            return true;
        }
    }

    public enum EventHandlerFunctions // all the things the Handler should/can do
    {
        InvalidCommand = 0,

        Init = 1,
        InvokeCommand,
        RequestCommand,
        UnInit,

        SymptomBasedDetectionModule = 100,
        EpidemyAlertModule,
        ImageProcessingModule,
        SymptomLearningModule,
        DataBaseModule,
    }

    public enum CoreKnownFunctions // this should be placed in Core, for everybody to add complexity needed
    {
        InvalidCommand = 0,

        DiagnosisUnInit = 1,
        DiagnosisRestart,
        DiagnosisTriggerDiagnostic,
        DiagnosisInvoke,

    }

    public enum SubModuleFunctions // all the functions for all the modules, should stay categorized!
    {
        InvalidCommand = 0,

        MachineLearningAsk = 1,
        MachineLearningGetResults,
        MachineLearningStoreResults,
        MachineLearningAdapt,

        CreateForm = 101,
        AskForFormResults,
        SaveFormResults,
        StartForm,

        ImageAddPhoto = 201,
        ImageComparePhoto,
        ImageStoreResults,
        ImageAdapt,

        EpidemyAskData = 301,
        EpidemyStoreData,
        EpidemyAlert,
        EpidemyCheckForAlert,

        DataBaseSaveData = 401,
        DataBaseDestroyData,
        DataBaseQueryData,
        DataBaseAlterData,

        CheckSigsForUser = 501,
        ReloadSigs = 502
    }
}
