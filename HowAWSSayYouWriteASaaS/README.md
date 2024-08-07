---
transition: "slide"
slideNumber: false
title: "How AWS say you should write a SaaS application"

---

### How do you go about writing a SaaS Application?
  
---

### TL;DR:

SaaS is not the same as a cloud native multi-tenant web application

---

You watch lots of videos and read the SaaS learning sections on the AWS and Microsoft web sites (see Resources)

---

![book](images/book.jpg)

[O'Reilly learning](https://learning.oreilly.com/library/view/building-multi-tenant-saas/9781098140632/)

---

### What we'll cover

- The Theory Behind SaaS
- SaaS Architecture Fundamentals
- Deployment Models
- Some implementation Ideas (the tech stuff)
- Guiding Principles

---

![Current](images/current.png)

[Image](https://learning.oreilly.com/library/view/building-multi-tenant-saas/9781098140632/ch01.html#fig_1_the_installed_software_model)

---

### What's wrong with these deployments?

- Customers with different versions
- Support always being "upgrade to latest"
- Bespoke versions of software
- Backdoor access to data repositories
- Environment issues
- Snowflaking via tweaks

---

![To The SaaS](images/to-the-saas.png)

[Image](https://learning.oreilly.com/library/view/building-multi-tenant-saas/9781098140632/ch01.html#fig_2_a_shared_infrastructure_saas_model)

---

![Control plane](images/controlplane.png)

[Image](https://learning.oreilly.com/library/view/building-multi-tenant-saas/9781098140632/ch01.html#fig_3_building_cross_cutting_saas_capabilities)

---

### The Control Plane

- The wrapper that lets your application code be application code
- Embed the application inside something that is generic and provides the other services
- Behind this barrier everything is taken care of for the customer

- (But the application isn't isolated and likely calls back to the control plane)

---

### [The meaningless of "multi-tenant"](https://learning.oreilly.com/library/view/building-multi-tenant-saas/9781098140632/ch01.html#:-:text=Redefining%20Multi-Tenancy)

- We need a different language to talk about the sharedness and isolation of resources
- And there's no meaning of single-tenant

---

### Multi-tenancy

![Multi-tenancy](images/multi-tenancy.png)

[Image](https://learning.oreilly.com/library/view/building-multi-tenant-saas/9781098140632/ch01.html#fig_5_multi_tenancy_with_shared_and_dedicated_resources)

---

### [At Its Core, SaaS Is a Business Model](https://learning.oreilly.com/library/view/building-multi-tenant-saas/9781098140632/ch01.html#at_its_core_saas_is_a_business_model)

[It should be clear that SaaS is very much about creating a technology, business and operational culture that is focused squarely on driving a distinct set of business outcomes. So, while it is tempting to think about SaaS though the lens of technology patterns and strategies, you should really be viewing SaaS more as a business model.](https://learning.oreilly.com/library/view/building-multi-tenant-saas/9781098140632/ch01.html#:-:text=By%20now%20you%20should,as%20a%20business%20model.)

---

### The SaaS Model offers

- [Agility](https://learning.oreilly.com/library/view/building-multi-tenant-saas/9781098140632/ch01.html#:-:text=Agility)
- [Operational Efficiency](https://learning.oreilly.com/library/view/building-multi-tenant-saas/9781098140632/ch01.html#:-:text=Operational%20efficien)
- [Innovation](https://learning.oreilly.com/library/view/building-multi-tenant-saas/9781098140632/ch01.html#:-:text=Innovation)
- [Frictonless Onboarding](https://learning.oreilly.com/library/view/building-multi-tenant-saas/9781098140632/ch01.html#:-:text=Frictionless%20onboarding)
- [Growth](https://learning.oreilly.com/library/view/building-multi-tenant-saas/9781098140632/ch01.html#:-:text=Growth)

---

### In Summary

[You are building a service and not a product.](https://learning.oreilly.com/library/view/building-multi-tenant-saas/9781098140632/ch01.html#:-:text=Building%20a%20Service%E2%80%94Not%20a%20Product)

---

Enough of the theory, back to the technology...

---

### [What's a tenant?](https://learning.oreilly.com/library/view/building-multi-tenant-saas/9781098140632/ch02.html#:-:text=Adding%20Tenancy%20to%20Your%20Architecture)

- A logical entity that owns resources
- It will have many uses in the control plane

- Typically carried as part of the token on the request

SaaS identity == Identity + Tenant ID

---

[Microsoft have a good page on this.](https://learn.microsoft.com/en-us/azure/architecture/guide/multitenant/considerations/tenancy-models#define-a-tenant)

---

### SaaS Architecture fundamentals

---

![Application and control planes](images/control-and-application-planes.png)

[Image](https://learning.oreilly.com/library/view/building-multi-tenant-saas/9781098140632/ch02.html#fig_4_saas_application_and_control_planes)

---

### The Control plane does (at least)

- [Onboarding / Offboarding](https://learning.oreilly.com/library/view/building-multi-tenant-saas/9781098140632/ch02.html#:-:text=Onboarding)
- [Identity](https://learning.oreilly.com/library/view/building-multi-tenant-saas/9781098140632/ch02.html#:-:text=Identity)
- [Metrics](https://learning.oreilly.com/library/view/building-multi-tenant-saas/9781098140632/ch02.html#:-:text=Metrics)
- [Billing](https://learning.oreilly.com/library/view/building-multi-tenant-saas/9781098140632/ch02.html#:-:text=Billing)
- [Tenant Management](https://learning.oreilly.com/library/view/building-multi-tenant-saas/9781098140632/ch02.html#:-:text=Tenant%20Management)

---

### The Application plane does (at least)

- [Tenant context](https://learning.oreilly.com/library/view/building-multi-tenant-saas/9781098140632/ch02.html#:-:text=Tenant%20Context)
- [Tenant isolation](https://learning.oreilly.com/library/view/building-multi-tenant-saas/9781098140632/ch02.html#:-:text=Tenant%20Isolation)
- [Data partitioning](https://learning.oreilly.com/library/view/building-multi-tenant-saas/9781098140632/ch02.html#:-:text=Data%20Partitioning)
- [Tenant routing](https://learning.oreilly.com/library/view/building-multi-tenant-saas/9781098140632/ch02.html#:-:text=Tenant%20Routing)

---

### The Grey area

- [Tiering](https://learning.oreilly.com/library/view/building-multi-tenant-saas/9781098140632/ch02.html#:-:text=Tiering)
- [Tenant/Tenant Admin/System Admin Users](https://learning.oreilly.com/library/view/building-multi-tenant-saas/9781098140632/ch02.html#:-:text=Tenant%2C%20Tenant%20Admin%2C%20and%20System%20Admin%20Users)
- [Tenant Provisioning](https://learning.oreilly.com/library/view/building-multi-tenant-saas/9781098140632/ch02.html#:-:text=Tenant%20Provisioning)

---

### SaaS Deployment Models

---

[The first hurdle I faced in this space was the absence of any precise terminology that accurately categorized the different patterns of multi-tenant deployments.](https://learning.oreilly.com/library/view/building-multi-tenant-saas/9781098140632/ch03.html#:-:text=The%20first%20hurdle,multi-tenant%20deployments.)

---

Resources like compute and storage can be independently pooled and siloed

---

![Siloed and pooled](images/siloed-and-pooled.png)

[Image](https://learning.oreilly.com/library/view/building-multi-tenant-saas/9781098140632/ch03.html#fig_2_siloed_and_pooled_resource_models)

---

### Full Stack Silo

- Full stack silo can be a good first step

Siloed == for the use of the tenant, likely isolated via cloud provider, IAM and application code

---

![Full stack silo](images/full-stack-silo.png)

[Image](https://learning.oreilly.com/library/view/building-multi-tenant-saas/9781098140632/ch03.html#fig_4_managing_and_operating_a_full_stack_silo)

---

### [Full Stack Silo Considerations](https://learning.oreilly.com/library/view/building-multi-tenant-saas/9781098140632/ch03.html#:-:text=Full%20Stack%20Silo%20Considerations)

- Control Plane Complexity
- Scaling impacts
- Cost considerations
- Routing
- Availability and blast radius
- Simpler cost attribution
- Isolation is easy via accounts or VPCs

---

### Full stack pool

Share everything!

---

![Full stack pool](images/full-stack-pool.png)

[Image](https://learning.oreilly.com/library/view/building-multi-tenant-saas/9781098140632/ch03.html#fig_7_a_full_stack_pool_model)

---

### And the flow of tenant context

![Tenant context](images/tenant-context.png)

[Image](https://learning.oreilly.com/library/view/building-multi-tenant-saas/9781098140632/ch03.html#fig_8_tenant_context_in_the_full_stack_pooled_environmen)

---

### [Full Stack Pool Considerations](https://learning.oreilly.com/library/view/building-multi-tenant-saas/9781098140632/ch03.html#:-:text=Full%20Stack%20Pool%20Considerations)

- Scale
- Isolation
- Availability and blast radius
- Noisy neighbour
- Cost attribution
- Operational Simplification

---

### [Mixed Mode](https://learning.oreilly.com/library/view/building-multi-tenant-saas/9781098140632/ch03.html#:-:text=The%20Mixed%20Mode%20Deployment%20Model)

Best of both worlds!

---

![Mixed mode](images/mixed-mode.png)

[Mixed Mode](https://learning.oreilly.com/library/view/building-multi-tenant-saas/9781098140632/ch03.html#fig_11_a_mixed_mode_deployment_model)

---

### [Pod deployment](https://learning.oreilly.com/library/view/building-multi-tenant-saas/9781098140632/ch03.html#:-:text=The%20Pod%20Deployment%20Model)

- Stamps

---

![Pod deployment](images/pod-deployments.png)

[Image](https://learning.oreilly.com/library/view/building-multi-tenant-saas/9781098140632/ch03.html#fig_12_a_pod_deployment_model)

---

### Some Implementation Bits and Pieces

It's hard to put all that high level talk into practice, so let's have a look at some of the details in various places.

---

### [SaaS Identity](https://learning.oreilly.com/library/view/building-multi-tenant-saas/9781098140632/ch04.html#fig_9_creating_a_logical_saas_identity)

There's YOU and the context in which you are working

---

![Saas Identity](images/saas-identity.png)

---

![Custom Claims](images/custom-claims.png)

[Image](https://learning.oreilly.com/library/view/building-multi-tenant-saas/9781098140632/ch04.html#fig_11_adding_tenant_custom_claims_to_a_jwt)

---

![Pass token](images/pass-token.png)

[Image](https://learning.oreilly.com/library/view/building-multi-tenant-saas/9781098140632/ch04.html#fig_13_passing_tokens_to_downstream_microservices)

---

### Beware

- Use custom claims judiciously
- You can avoid adding to the token and just call a service to resolve

---

![Federation](images/federation.png)

[Image](https://learning.oreilly.com/library/view/building-multi-tenant-saas/9781098140632/ch04.html#fig_15_supporting_externally_hosted_identity_providers)

---

### [Tenant Management](https://learning.oreilly.com/library/view/building-multi-tenant-saas/9781098140632/ch04.html#fig_15_supporting_externally_hosted_identity_providers)

---

![Tenant Management](images/tenant-management.png)

[Image](https://learning.oreilly.com/library/view/building-multi-tenant-saas/9781098140632/ch05.html#fig_1_tenant_management_s_influence)

---

![Authentication Flow](images/auth-flow.png)

[Image](https://learning.oreilly.com/library/view/building-multi-tenant-saas/9781098140632/ch06.html#fig_7_a_sample_multi_tenant_authentication_flow)

---

![Tenant Routing](images/tenant-routing.png)

[Image](https://learning.oreilly.com/library/view/building-multi-tenant-saas/9781098140632/ch06.html#fig_8_tenant_routing_basics)

---

### Implement how?

- Containers, possibly leading to Kubernetes ([AWS reference model](https://github.com/aws-samples/aws-saas-factory-eks-reference-architecture?tab=readme-ov-file))
- Serverless

---

![Containers or serverless](images/containers-or-serverless.png)

[Image](https://learning.oreilly.com/library/view/building-multi-tenant-saas/9781098140632/ch07.html#fig_8_compute_and_service_design)

---

### The Importance of Tenant Aware Metrics

---

![Tenant Aware Metrics](images/tenant-aware-metrics.png)

[Image](https://learning.oreilly.com/library/view/building-multi-tenant-saas/9781098140632/ch07.html#fig_9_surfacing_tenant_aware_service_metrics)

---

### Getting Tenant information inside the application code

---

``` python
def query_orders(self, status):
  # get tenant context
  auth_header = request.headers.get('Authorization')
  token = auth_header.split(" ")
  if (token[0] != "Bearer")
    raise Exception('No bearer token in request')
  bearer_token = token[1]
  decoded_jwt = jwt.decode(bearer_token, "secret",
                   algorithms=["HS256"])
  tenant_id = decoded_jwt['tenantId']
  tenant_tier = decoded_jwt['tenantTier']
 
  # query for orders with a specific status
  logger.info("Finding orders with the status of %s", status)
```

---

### Multi-tenant isolation

- Multi-tenant isolation happens via application code and via platform security mechanisms

- potentially using [sidecars](https://learning.oreilly.com/library/view/building-multi-tenant-saas/9781098140632/ch07.html#fig_12_using_sidecars_for_horizontal_concepts) or aspects

---

``` Python
def query_orders(self, status):
  # get database client (DynamoDB) with tenant scoped credentials
  sts = boto3.client('sts')
  
  # get credentials based on tenant scope policy
  tenant_credentials = sts.assume_role(
    RoleArn = os.environ.get('IDENTITY_ROLE'),
    RoleSessionName = tenant_id,
    Policy = scoped_policy,
    DurationSeconds = 1000
  )
  ...
```

---

### Data Partitioning

---

![Data partitioning](images/data-partitioning.png)

[Image](https://learning.oreilly.com/library/view/building-multi-tenant-saas/9781098140632/ch08.html#fig_1_siloed_and_pooled_data_partitioning_models)

---

- [Workloads, SLAs and User Experience](https://learning.oreilly.com/library/view/building-multi-tenant-saas/9781098140632/ch08.html#:-:text=Workloads%2C%20SLAs%2C%20and%20Experience)
- [Blast Radius](https://learning.oreilly.com/library/view/building-multi-tenant-saas/9781098140632/ch08.html#:-:text=Blast%20Radius)
- [Isolation](https://learning.oreilly.com/library/view/building-multi-tenant-saas/9781098140632/ch08.html#:-:text=The%20Influence%20of%20Isolation)
- [Operations](https://learning.oreilly.com/library/view/building-multi-tenant-saas/9781098140632/ch08.html#:-:text=Management%20and%20Operations)
- [Rightsizing](https://learning.oreilly.com/library/view/building-multi-tenant-saas/9781098140632/ch08.html#:-:text=The%20Rightsizing%20Challenge)
- [Throughput and Throttling](https://learning.oreilly.com/library/view/building-multi-tenant-saas/9781098140632/ch08.html#:-:text=Throughput%20and%20Throttling)
- [Serverless Storage](https://learning.oreilly.com/library/view/building-multi-tenant-saas/9781098140632/ch08.html#:-:text=Serverless%20Storage)

---

### More on Data Isolation

---

![Resource Isolation](images/resource-isolation.png)

[Image](https://learning.oreilly.com/library/view/building-multi-tenant-saas/9781098140632/ch09.html#fig_4_categorizing_isolation_models)

---

- Application enforced isolation
- Infrastructure isolation
- Deployment time and runtime isolation
- Isolation through interception
- Scaling

---

### Have Enough Metrics to Attribute Costs

---

![Attribute Costs](images/attribute-costs.png)

[IMage](https://learning.oreilly.com/library/view/building-multi-tenant-saas/9781098140632/ch12.html#fig_3_attributing_consumption_of_pooled_resources)

---

### Migration Strategies

---

![Balancing act](images/balancing-act.png)

[Image](https://learning.oreilly.com/library/view/building-multi-tenant-saas/9781098140632/ch13.html#fig_1_the_migration_balancing_act)

---

![Migrate or modernize](images/migrate-modernize.png)

[Image](https://learning.oreilly.com/library/view/building-multi-tenant-saas/9781098140632/ch13.html#fig_2_migration_timing_trade_offs)

---

![Fish model](images/fish-model.png)

[Image](https://learning.oreilly.com/library/view/building-multi-tenant-saas/9781098140632/ch13.html#fig_3_the_migration_fish_model)

---

### Migration styles (foundation)

[Every migration starts with a control plane](https://learning.oreilly.com/library/view/building-multi-tenant-saas/9781098140632/ch13.html#:-:text=The%20Foundation)

---

### Migration styles

- [Silo lift and shift](https://learning.oreilly.com/library/view/building-multi-tenant-saas/9781098140632/ch13.html#:-:text=Silo%20Lift-and-Shift)
- [Layered migration](https://learning.oreilly.com/library/view/building-multi-tenant-saas/9781098140632/ch13.html#:-:text=Layered%20Migration)
- [Service-by-service migration](https://learning.oreilly.com/library/view/building-multi-tenant-saas/9781098140632/ch13.html#:-:text=Service-by-Service%20Migration)

---

![Silo lift and shift](images/silo-lift-and-shift.png)

[Image](https://learning.oreilly.com/library/view/building-multi-tenant-saas/9781098140632/ch13.html#fig_5_silo_lift_and_shift_migration)

---

### [Guiding Principles](https://learning.oreilly.com/library/view/building-multi-tenant-saas/9781098140632/ch17.html#:-:text=Chapter%2017.%20Guiding%20Principles)

- [Build a Vision and a Strategy](https://learning.oreilly.com/library/view/building-multi-tenant-saas/9781098140632/ch17.html#:-:text=Build%20a%20Business%20Model%20and%20Strategy)
- [Focus on Efficiency](https://learning.oreilly.com/library/view/building-multi-tenant-saas/9781098140632/ch17.html#:-:text=A%20Clear%20Focus%20on%20Efficiency)
- [Avoid the Tech-First Trap](https://learning.oreilly.com/library/view/building-multi-tenant-saas/9781098140632/ch17.html#:-:text=Avoiding%20the%20Tech-First%20Trap)
- [Think Beyond Cost Savings](https://learning.oreilly.com/library/view/building-multi-tenant-saas/9781098140632/ch17.html#:-:text=Thinking%20Beyond%20Cost%20Savings)
- [Be All-in with SaaS](https://learning.oreilly.com/library/view/building-multi-tenant-saas/9781098140632/ch17.html#:-:text=Be%20All-In%20with%20SaaS)
- [Adopt a Service Centric Mindset](https://learning.oreilly.com/library/view/building-multi-tenant-saas/9781098140632/ch17.html#:-:text=Adopt%20a%20Service-Centric%20Mindset)
- [Think beyond existing Tenant Personas](https://learning.oreilly.com/library/view/building-multi-tenant-saas/9781098140632/ch17.html#:-:text=Think%20Beyond%20Existing%20Tenant%20Personas)

---

### [Core Technical Considerations](https://learning.oreilly.com/library/view/building-multi-tenant-saas/9781098140632/ch17.html#:-:text=Core%20Technical%20Considerations)

- [No One-Size-Fits-All model](https://learning.oreilly.com/library/view/building-multi-tenant-saas/9781098140632/ch17.html#:-:text=No%20One-Size-Fits-All%20Model)
- [Protect the Multi-Tenant Principles](https://learning.oreilly.com/library/view/building-multi-tenant-saas/9781098140632/ch17.html#:-:text=Protect%20the%20Multi-Tenant%20Principles)
- [Build your Multi-tenant Foundation on Day one](https://learning.oreilly.com/library/view/building-multi-tenant-saas/9781098140632/ch17.html#:-:text=Build%20Your%20Multi-Tenant%20Foundation%20on%20Day%20One)
- [Avoid One-off customization](https://learning.oreilly.com/library/view/building-multi-tenant-saas/9781098140632/ch17.html#:-:text=Avoid%20One-Off%20Customization)
- [Measure your Multi-Tenant Architecture](https://learning.oreilly.com/library/view/building-multi-tenant-saas/9781098140632/ch17.html#:-:text=Measure%20Your%20Multi-Tenant%20Architecture)
- [Streamline the Developer Experience](https://learning.oreilly.com/library/view/building-multi-tenant-saas/9781098140632/ch17.html#:-:text=Streamline%20the%20Developer%20Experience)

---

### Resources

- [AWS SaaS Factory](https://aws.amazon.com/blogs/apn/introducing-aws-saas-factory-to-help-isvs-accelerate-saas-adoption/)
- [SaaS Factory Live](https://www.youtube.com/@saas-on-aws/streams)
- #sig-cloud from 11th March onwards

---

### And Microsoft's model

- [Microsoft Learn: SaaS and multitenant solution architecture](https://learn.microsoft.com/en-us/azure/architecture/guide/saas-multitenant-solution-architecture/) 
- [Azure SaaS Development Kit](https://github.com/Azure/azure-saas)
- [Microsoft SaaS academy](https://www.microsoft.com/en-us/saas-academy/main)

---
