﻿using MLOps.NET.Catalogs;
using MLOps.NET.Docker.Interfaces;
using MLOps.NET.Kubernetes.Interfaces;
using MLOps.NET.Services;
using MLOps.NET.Storage;
using MLOps.NET.Storage.Interfaces;
using MLOps.NET.Utilities;
using System;

namespace MLOps.NET
{
    ///<inheritdoc cref="IMLOpsContext"/>
    public class MLOpsContext : IMLOpsContext
    {
        internal MLOpsContext(IModelRepository modelRepository,
            IExperimentRepository experimentRepository,
            IRunRepository runRepository,
            IDataRepository dataRepository,
            IMetricRepository metricRepository,
            IConfusionMatrixRepository confusionMatrixRepository,
            IHyperParameterRepository hyperParameterRepository,
            IDeploymentRepository deploymentRepository,
            IDockerContext dockerContext,
            IKubernetesContext kubernetesContext)
        {
            if (modelRepository == null) throw new ArgumentNullException(nameof(modelRepository));
            if (experimentRepository == null) throw new ArgumentNullException(nameof(experimentRepository));
            if (runRepository == null) throw new ArgumentNullException(nameof(runRepository));
            if (dataRepository == null) throw new ArgumentNullException(nameof(dataRepository));
            if (metricRepository == null) throw new ArgumentNullException(nameof(metricRepository));
            if (confusionMatrixRepository == null) throw new ArgumentNullException(nameof(confusionMatrixRepository));
            if (hyperParameterRepository == null) throw new ArgumentNullException(nameof(hyperParameterRepository));
            if (deploymentRepository == null) throw new ArgumentNullException(nameof(deploymentRepository));

            this.LifeCycle = new LifeCycleCatalog(experimentRepository, runRepository, new Clock(), new PackageDependencyIdentifier(), new SchemaGenerator());
            this.Data = new DataCatalog(dataRepository);
            this.Evaluation = new EvaluationCatalog(metricRepository, confusionMatrixRepository);
            this.Model = new ModelCatalog(modelRepository, runRepository);
            this.Training = new TrainingCatalog(hyperParameterRepository);
            this.Deployment = new DeploymentCatalog(deploymentRepository, modelRepository, experimentRepository, runRepository, dockerContext, kubernetesContext, new SchemaGenerator());
        }

        ///<inheritdoc cref="IMLOpsContext"/>
        public LifeCycleCatalog LifeCycle { get; private set; }

        ///<inheritdoc cref="IMLOpsContext"/>
        public DataCatalog Data { get; set; }

        ///<inheritdoc cref="IMLOpsContext"/>
        public EvaluationCatalog Evaluation { get; private set; }

        ///<inheritdoc cref="IMLOpsContext"/>
        public ModelCatalog Model { get; private set; }

        ///<inheritdoc cref="IMLOpsContext"/>
        public TrainingCatalog Training { get; private set; }

        ///<inheritdoc cref="IMLOpsContext"/>
        public DeploymentCatalog Deployment { get; set; }

    }
}
