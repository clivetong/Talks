---
transition: "slide"
slideNumber: false
title: "How AWS say you should write a SaaS application"

---

### How do you go about writing a SaaS Application?
  
---

![book](images/book.jpg)

[Here on O'Reilly learning](https://learning.oreilly.com/library/view/building-multi-tenant-saas/9781098140632/)

---

### What we'll cover

- The Theory Behind SaaS
- Architecture Fundamentals
- Deployment Models
- Some implementation ideas
- Guiding Principles

---

![Current](images/current.png)

[Image](https://learning.oreilly.com/library/view/building-multi-tenant-saas/9781098140632/ch01.html#fig_1_the_installed_software_model)

---

### What's wrong with current deployments?

- Customers with different versions
- Support always being "upgrade to latest"
- Bespoke versions of software
- Backdoor access to data repositories
- Snowflaking via tweaks

---

### The meaningless of "multi-tenant"

- We need a different language to talk about the sharedness and isolation of resources

---

### The wrapper (control plane)

- Embed the application inside something that is generic and provides the other services
- Behind this barrier everything is taken care of for the customer

- (But the application isn't isolated)

---

### The control plane

![Control plane](images/controlplane.png)

[Image](https://learning.oreilly.com/library/view/building-multi-tenant-saas/9781098140632/ch01.html#fig_3_building_cross_cutting_saas_capabilities)

---

![Application and control planes](images/control-and-application-planes.png)

[Image](https://learning.oreilly.com/library/view/building-multi-tenant-saas/9781098140632/ch02.html#fig_4_saas_application_and_control_planes)

---

### Multi-tenancy

![Multi-tenancy](images/multi-tenancy.png)

[Image](https://learning.oreilly.com/library/view/building-multi-tenant-saas/9781098140632/ch01.html#fig_5_multi_tenancy_with_shared_and_dedicated_resources)

---

[It should be clear that SaaS is very much about creating a technology, business and operational culture that is focused squarely on driving a distinct et of business outcomes. So, while it is tempting to think about SaaS though the lens of technologypatterns and strategies, you should really be viewing SaaS more as a business model.](https://learning.oreilly.com/library/view/building-multi-tenant-saas/9781098140632/ch01.html#:-:text=By%20now%20you%20should,as%20a%20business%20model.)

---

### The SaaS Model offers

- Agility
- Operational efficiency
- Innovation
- Frictonless onboarding
- Growth

---

### In summary...

You are building a service and not a product.

---

Enough of the theory, back to the technology...

---

### What's a tenant?

- Something that owns resources
- Typically carried as part of the token on the request

SaaS identity == Identity + Tenant ID

---

### Architecture fundamentals

---

### The Control plane does...

- Onboarding
- Identity
- Metrics
- Billing
- Tenant management

---

### The Application plane does...

- Tenant context
- Tenant isolation
- Data partitioning
- Tenant routing

---

### The Grey area...

- Tiering
- Tenant/Tenant Admin/System Admin Users
- Tenant Provisioning

---

[The first hurdle I faced in this space was the absense of any precise terminology that accurately categorized the different patterns of multi-tenant deployments.](https://learning.oreilly.com/library/view/building-multi-tenant-saas/9781098140632/ch03.html#:-:text=The%20first%20hurdle,multi-tenant%20deployments.)

---

### The main words

- Resources like compute and storage can be independently pooled and siloed

- Full stack silo can be a good first step

Siloed == for the use of the tenant, likely isolated via cloud provider, IAM and application code

---

![Siloed and pooled](images/siloed-and-pooled.png)

[Image](https://learning.oreilly.com/library/view/building-multi-tenant-saas/9781098140632/ch03.html#fig_2_siloed_and_pooled_resource_models)

---

### Full Stack Silo

![Full stack silo](images/full-stack-silo.png)

[Image](https://learning.oreilly.com/library/view/building-multi-tenant-saas/9781098140632/ch03.html#fig_4_managing_and_operating_a_full_stack_silo)

---

### Comments on Full Stack Silo

- Scaling impacts
- Cost considerations
- Routing
- Availability and blast radius
- Simpler cost attribution
- But isolation is easy via accounts or VPCs

---

### Full stack pool

![Full stack pool](images/full-stack-pool.png)

[Image](https://learning.oreilly.com/library/view/building-multi-tenant-saas/9781098140632/ch03.html#fig_7_a_full_stack_pool_model)

---

### And the flow of tenant context

![Tenant context](images/tenant-context.png)

[Image](https://learning.oreilly.com/library/view/building-multi-tenant-saas/9781098140632/ch03.html#fig_8_tenant_context_in_the_full_stack_pooled_environmen)

---

### Comments on Full Stack Pool

- Scale
- Isolation
- Availability and blast radius
- Noisy neighbour
- Cost attribution
- Operational Simplification

---

### Mixed Mode

![Mixed mode](images/mixed-mode.png)

[Image](https://learning.oreilly.com/library/view/building-multi-tenant-saas/9781098140632/ch03.html#fig_11_a_mixed_mode_deployment_model)

---

### Pod deployment

- Stamps

---

### Some Implementation Bits and Pieces

It's hard to put all that high level talk into practice, so let's have a look at some of the details in various places.

---

### Guiding Principles

- Build a Vision and a Strategy
- Focus on Efficiency
- Avoid the Tech-First Trap
- Think Beyond Cost Savings
- Be All-in with SaaS
- Adopt a Service Centric Mindset
- Think beyond existing Tenant Personas

---

### Core Technical COnsiderations

- No One-Size-Fits-All model
- Protect the Multi-Tenant Principles
- Build your Multi_tenant Foundation on Day one
- Avoid One-off customization
- Measure your Multi-Tenant Architecture
- Streamline the Developer Experience

---

### And Microsoft's model

---

### Resources

