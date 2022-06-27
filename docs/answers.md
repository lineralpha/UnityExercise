 Q1.) Your team has just deployed a new build. The application starts crashing because it’s
running out of memory after several hours. The team suspects a memory leak has been
introduced in one of the apps dependencies.
a.) How would you help the web team diagnose and fix the problem?
b.) How would you stop issues like this making its way into production?

### A: Memory leak.  

First thing to look is the app's logs. Search if there is any out-of-memory exception in the logs. Be aware that the stacktrace reporting out-ot-memory exception may not be where leaks happen, but where/when the app's new memory allocation fails due to insufficient memory.

On Linux, the first tool I would use for memory leak detection is "htop/top" which reports CPU and memory usage per process. It is easy to sort the list by CPU or memory usage or any other columns. To confirm memory leak problem, use this tool to watch if the app process has increasing memory footprint overtime. Also "seige" is a pretty good load testing tool. Use seige to speed up the occurrence of memory leak as needed. There are other useful tools (vmstat/free etc.) too that can report memory usage on the system.

Depending on the tech stack the app is developed, there are debugger tools that can be used to analyze memory dump taken from the suspected process, for example, dotnet-dump/vsdebugger/windbg for AspNetCore apps. One thing to look is the GC roots in garbage collector (I believe JVM and .NET have the similar concept). All unreleased objects have traces left in GC roots. And usually by checking GC roots for objects that hold large block of memory, objects leaking memory can be identified. One thing to note is, objects may just allocate small block of memory each time, but accumulatively overtime, they hold a lot memory if leak happens.

To prevent from memory leaks, in addition to functional testing, app should pass mandatory/proper load/pressure testing before deployed to production. Many leak issues can be found and fixed timely during developing or staging phases if good load testing is performed. Coding review is also an important activity to spot memory leaks. An additional pair of eagle eyes is always an asset to the team.

===

Q2.) Describe some of the advantages and disadvantages of SQL and NoSQL data stores,
and the general use cases you think they each fit.

### A: SQL vs. NoSQL

There are lots of comparison summary with regard to SQL vs. NoSQL. I think some of the important aspects are data transaction/consistency, data schema/structure, query language, scalability, and application scenarios.

1. transaction/consistency: SQL databases guarantee data transaction and ACID consistency; while NoSQL follows CAP property and provides BASE (eventual) consistency.

2. data schema/structure: SQL databases enforce well-formed data schema. Data in SQL are rational and well structured. While NoSQL is good for schemaless and unstructured data, like documents, key-values, graphs, or wide-column data.

3. query language:query language for SQL is powerful and well designed for structured data. Due to the nature of unstructured data, there is no one single query language for all NoSQL databases. 

4. scalability. SQL databases are vertically scalable in most situations. NoSQL are horizontally scalable, thus it is easy to perform sharding to adapt higher data traffic. Horizontal scaling has a greater overall capacity than vertical scaling, making NoSQL the preferred choice for large and frequently changing data sets.

5. application scenarios: Due to the above critical differences, SQL and NoSQL have their own application scenarios. For example, applications like banks that critically require data transaction and ACID consistency should use SQL databases. Applications like social medias that handle large amount of data/messages but are OK with eventual consistency would prefer NoSQL.

===

Q3.) Describe the workflows, ecosystems, and technology you would use/create to ensure the
dual goals of GDPR/privacy compliance and data accessibility for business/application
needs.

## A: GDPR/privacy compliance, accessibility etc.

There are lots of "GDPR guidelines/best practices" available online. There are process and toolset that company/team will need in order to achieve GDPR goals. From engineering perspective, I would suggest the following.

1. Process
    - Data classification. All client data collected have to be classified to ensure data confidentiality, integrity, and availability. Particularly, when PII data are appropriately classified and flagged, it is easier to ensure security and privacy controls are adequate.
    - Privacy impact assessment. Take privacy-by-design approach to identify and assess the risks that could arise from the collecting, use and handling of the above classified PII data. Be aware of the data flow boundary to ensure data can only be accessible to those who need them and are responsible.
    - Test data breach. Just like application requires functional testing. Auditing data privacy and testing data breach/vulnerability is a required signoff for applications.
    - Test accessibility. This is obvious.
    - Monitor and audit GDPR compliance: This is to conduct regular audits of privacy protection practices, record how data are held, processed and transferred.
    - Document and maintain privacy policies, procedures and processes.

2. Toolset. 
    There are lots of GDPR compliance tools out there. This is not a list of "best tools", but some of the tools I am familiar with to some extent:
    - OneTrust.com - for GDPR compliance. 
    - Let's Encrypt - a certificate authority service providing TLS certificates to websites.
    - Accessibility tools:
        - GoogleChrome Lighthouse: a chrome extension to test webpage's performance, accessibility, etc.
        - NVDA - a screenreader tool for testing webpages (also desktop apps)
        - JAWS Inspect - an other screenreader tool I have used in the past.
        - Color Contrast Analyzer (CCA) - a tool to test webpage's contrast ratio for accessibility compliance.
        - WCAG Color contrast checker - a chrome extension.

===

4.) What’s your pre and post-deployment checklist for a new application, inclusive of the
entire stack (assume the infra/cloud provider of your choice)?

## A: Application deployment checklists

1. Pre-deployment checklist
    - production environment preparation and signoff
      - Azure account/resource group/app service plan/app configuration/environment setup etc.
      - AKS setup if deploying to AKS
      - SSL/TLS certificates
      - automation tools/scripts readiness. Particularly, deploying pipeline.
      - protect sensitive data such as account info/password/database connection string, etc.
      - prepare and create Azure app dashboard for app heath check post-deployment
        - add app health metric charts for the indicators/alerts needing to be monitored.
        - add app logs query, etc.
    - testing signoffs:
      - functional testing signoff
      - load testing signoff
      - performance testing signoff
    - verify deployment in staging environment
    - SLO signoffs against SLAs in staging environment
    - prepare for any traffic spikes
    - audit third-party code and licenses
    - disaster rollback and recovery plan
      - ensure to be able to quickly roll back to previous version.

2. Post-deployment checklist
    In the Azure app dashboard,
    - monitor and audit application logs
    - monitor alerts, exceptions, and key performance indicators
    - monitor application queues/caches, database queries, etc.
    - monitor resource usage and provisioning
    - monitor compliance violation
    - postmortem meetings
