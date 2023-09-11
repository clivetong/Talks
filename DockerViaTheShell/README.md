# What is there inside a docker container?

- run docker hello world
  docker run hello-world

- export and untar

Note the export is gives us everything we need to run the container, including the various layers
and details about the entry point when the container runs.

```
  docker save hello-world:latest -o foo.tar
  tar tvf foo.tar
```

- look at the contents (and notice that this has no layers)
  The hello-world is actually a binary that doesn't need any of the user space linux libraries

```
clive@clive-VirtualBox:~/docker-play/cdccdf50922d90e847e097347de49119be0f17c18b4a2d98da9919fa5884479d$ objdump --disassemble hello
hello:     file format elf64-x86-64
Disassembly of section .text:
0000000000400150 <.text>:
400150:    48 8d 15 69 00 00 00     lea    0x69(%rip),%rdx        # 0x4001c0
400157:    48 83 ec 08              sub    $0x8,%rsp
40015b:    be 01 00 00 00           mov    $0x1,%esi
400160:    bf 01 00 00 00           mov    $0x1,%edi
400165:    b9 27 03 00 00           mov    $0x327,%ecx
40016a:    31 c0                    xor    %eax,%eax
40016c:    e8 0f 00 00 00           callq  0x400180
400171:    5a                       pop    %rdx
400172:    31 f6                    xor    %esi,%esi
400174:    bf 3c 00 00 00           mov    $0x3c,%edi
400179:    31 c0                    xor    %eax,%eax
40017b:    e9 00 00 00 00           jmpq   0x400180
400180:    48 89 f8                 mov    %rdi,%rax
400183:    48 89 f7                 mov    %rsi,%rdi
400186:    48 89 d6                 mov    %rdx,%rsi
400189:    48 89 ca                 mov    %rcx,%rdx
40018c:    4d 89 c2                 mov    %r8,%r10
40018f:    4d 89 c8                 mov    %r9,%r8
400192:    4c 8b 4c 24 08           mov    0x8(%rsp),%r9
400197:    0f 05                    syscall
400199:    48 3d 01 f0 ff ff        cmp    $0xfffffffffffff001,%rax
40019f:    0f 83 0b 00 00 00        jae    0x4001b0
4001a5:    c3                       retq
4001a6:    66 2e 0f 1f 84 00 00     nopw   %cs:0x0(%rax,%rax,1)
4001ad:    00 00 00
4001b0:    48 f7 d8                 neg    %rax
4001b3:    64 89 04 25 fc ff ff     mov    %eax,%fs:0xfffffffffffffffc
4001ba:    ff
4001bb:    48 83 c8 ff              or     $0xffffffffffffffff,%rax
4001bf:    c3                       retq
```

```
strace ./hello
```

- build something that does need some Linux user space
  Write a Dockerfile that uses bash to print something to the output

- notice that now there are layers in the Docker messages

- take that apart and find the tarred layers inside the existing tar

# [Namespaces](https://www.youtube.com/watch?v=0kJPa-1FuoI)

- quick introduction to the 7 namespaces

- /proc/\$\$/ns/ and readlink

```

    ls /proc/$$/ns
    readlink /proc/$$/ns/uts

```

Mount was the original namespace added, but lets go for something simpler

- show that inside the kernel each process (task) has a proxy for the various namespaces

  - The kernel handles a proxy for the namespaces: https://github.com/torvalds/linux/blob/master/kernel/nsproxy.c#L29
  - The main namespace copy mechanism: https://github.com/torvalds/linux/blob/master/kernel/nsproxy.c#L60
  - They are switched into the task struct: https://github.com/torvalds/linux/blob/master/kernel/nsproxy.c#L213
  - For example the setns syscall: https://github.com/torvalds/linux/blob/master/kernel/nsproxy.c#L233
  - The namespace proxy is associated with the task: https://github.com/torvalds/linux/blob/a2953204b576ea3ba4afd07b917811d50fc49778/include/linux/sched.h#L910

  - add see where this indirection happens: https://github.com/torvalds/linux/search?q=nsproxy&unscoped_q=nsproxy

now we go namespace by namespace to try to show how this all really works

- UTS
- walk through the UTS namespace in two bash windows showing their independence

  - unshare to set up a new namespace

```

sudo unshare -u bash
hostname bananaman
hostname

```

    - nsenter to enter a namespace (but can target a process)

```

ps # in the new namespace process
sudo nsenter -a -t ...pid...

```

- pid

Freeze processes and move cross machine (so need to impersonate PIDs)

Process 1 is special in Linux world (init) and hardwired into the OS as the zombie waiter

Do this inside and outside a container

```

echo \$\$

```

The shell inside the container will have PID 1

When running `ps aux` inside a container it will list all processes, since it gathers them from the parent's mounted [/proc](http://man7.org/linux/man-pages/man5/proc.5.html) filesystem

```

strace ps

```

Use PID NS in conjunction with MOUNT NS and mount container's /proc filesystem 
```

mount -t proc proc /proc

```

- net

Own ARP and Routing information

```

ip netns add red
ip netns add blue
ip netns

```

Network interfaces

```

ip link

```

Run inside the namespace

```

ip netns exec red ip link
ip -n red link

```

Do the same with `route` and `arp`

Use a virtual cable

```

ip link add veth-red type veth peer name veth-blue
ip link set veth-red netns red
ip link set veth-blue netns blue

```

And assign ips

```

ip -n red addr add 192.168.15.1 dev veth-red
ip -n blue addr add 192.168.15.2 dev veth-blue

```

and bring them up

```

ip -n red link set veth-red up
ip -n blue link set veth-blue up

```

and now ping works

```

ip netns exec red ping 192.168.15.2

```

and see it has identified it's neighbour

```

ip netns exec red arp

```

Lookign on the host we see nothing

```

arp

```

To kill one end (And hence the other end too)

```

ip -n red link del veth-red

```

Add a BRIDGE here

- mount

Overlay

```

mkdir overlay-play
cd overlay-play
mkdir lower
mkdir upper
mkdir working
mkdir union
sudo mount -t overlay -o lowerdir=lower,upperdir=upper,workdir=working none union
cat > lower/play1
^D
ls union
cat > union/play3
^D
ls -l union
ls -l upper

```

- user

- ipc

- cgroup [Blog post](https://jvns.ca/blog/2016/10/10/what-even-is-a-container/)

* Now we have what we need
  - Set up the namespace to isolate the hello-world application
  - run hello world

# Running the container - see https://ilearnedhowto.wordpress.com/tag/unshare/
