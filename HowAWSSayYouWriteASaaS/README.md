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

- What does SaaS mean?
- Common Language for talking about things

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

### And Microsoft's model

---

### Resources

