<script lang="ts">
  import CircleUser from "@lucide/svelte/icons/circle-user";
  import Folder from "@lucide/svelte/icons/folder";
  import GraduationCap from "@lucide/svelte/icons/graduation-cap";
  import Mail from "@lucide/svelte/icons/mail";
  import Settings from "@lucide/svelte/icons/settings";
  import ShoppingCart from "@lucide/svelte/icons/shopping-cart";
  import UserCog from "@lucide/svelte/icons/user-cog";

  import {
    Avatar,
    AvatarFallback,
    AvatarImage,
  } from "$lib/components/ui/avatar";
  import {
    Card,
    CardContent,
    CardHeader,
    CardTitle,
  } from "$lib/components/ui/card";
  import { Input } from "$lib/components/ui/input";
  import { Separator } from "$lib/components/ui/separator";

  import SidebarButton from "$lib/components/pages/dashboard/SidebarButton.svelte";
  import type { TRAPI } from "../../../../shared/typings/api";
  import { api } from "$lib/modules/API";
  import type { Unpacked } from "$lib/utils";
  import type { AdminProps } from "$lib/components/pages/admin/helpers";

  let user = $state<TRAPI.Profile | null>(null);

  type ProfileFormState = {
    name: string;
    email: string;
    avatar: string;
    system: string;
    subsystem: string;
    gradDate: string;
    shirtSize: string;
    hazingStatus: boolean;
    feeStatus: boolean;
  };

  let data = $state(<ProfileFormState>{
    name: "Car McCarface",
    email: "cmccarface1@lsu.edu",
    avatar: "",
    system: "chassis",
    subsystem: "battery",
    gradDate: "2026-05-15",
    shirtSize: "L",
    hazingStatus: false,
    feeStatus: false,
    });

  $effect(() => {
    (async () => {
      const res = await api.fetchCurrentUser();
      user = res.data;
      if (user) {
        data.name = user.name;
        data.email = user.email;
        data.avatar = user.avatar ?? "";
        data.system = user.system ?? "";
        data.subsystem = user.subsystem ?? "";
        data.gradDate = user.gradYear?.toString() || "";
        data.shirtSize = user.shirtSize ?? "";
        data.hazingStatus = user.hazingStatus;
        data.feeStatus = user.feeStatus;
      } else {
        alert("Failed to fetch user data");
      }
    })();
  });

  let disabled = $state(true);
  let isEditing = $state(false);

  function handleEditProfile() {
    disabled = false;
    isEditing = true;
  }

  async function handleSaveProfile() {
    disabled = true;
    isEditing = false;
    const gradYear = Number.parseInt(data.gradDate, 10);
    if (user?.id) {
      await api.updateUser(user.id, {
        name: data.name,
        email: data.email,
        system: data.system ? (data.system as TRAPI.User["system"]) : undefined,
        subsystem: data.subsystem
          ? (data.subsystem as TRAPI.User["subsystem"])
          : undefined,
        gradYear: Number.isNaN(gradYear) ? undefined : gradYear,
        shirtSize: data.shirtSize
          ? (data.shirtSize as TRAPI.User["shirtSize"])
          : undefined,
        hazingStatus: data.hazingStatus,
        feeStatus: data.feeStatus,
      });
    } else if (user === null) {
        data.name = data.name;
        data.email = data.email;
        data.system = data.system;
        data.subsystem = data.subsystem;
        data.gradDate = data.gradDate;
        data.shirtSize = data.shirtSize;
        data.hazingStatus = data.hazingStatus;
        data.feeStatus = data.feeStatus;
    }
  }

  function handleChangeAvatar() {
    alert("Change Avatar clicked");
  }

  function handleChangePassword() {
    alert("Change Password clicked");
  }
</script>

<div class="flex flex-col xl:flex-row w-full xl:h-dvh">
  <!-- for accessibility scoring on dashboard -->
  <h1 class="hidden">Profile</h1>

  <!-- Sidebar -->
  <Card
    class="w-full xl:max-w-xs xl:dvh rounded-none bg-accent shadow-none gap-2"
  >
    <CardHeader class="xl:mt-16.75">
      <CardTitle>Profile</CardTitle>
    </CardHeader>
    <CardContent>
      <div class="flex flex-col gap-2">
        <SidebarButton title="Projects" url="/projects" icon={Folder} />
        <SidebarButton title="Orders" url="/orders" icon={ShoppingCart} />
        <SidebarButton title="Inbox" url="/mail" icon={Mail} />
        <SidebarButton title="Profile" url="/profile" icon={CircleUser} />
        <SidebarButton title="Settings" url="/settings" icon={Settings} />
      </div>
    </CardContent>
    <CardHeader class="mt-4">
      <CardTitle>Quick Links</CardTitle>
    </CardHeader>
    <CardContent class="h-full">
      <div class="flex flex-col gap-2 h-full">
        <SidebarButton
          title="Wiki"
          url="https://wiki.formulalsu.org"
          icon={GraduationCap}
        />
        <div class="grow"></div>
        <SidebarButton title="Admin" url="/admin" icon={UserCog} />
      </div>
    </CardContent>
  </Card>

  <!-- Main Dashboard -->
  <div class="sm:px-4 xl:p-8 grow mt-16 overflow-auto">
    <div class="flex flex-col lg:flex-row gap-4 h-full">
      <!-- Profile Card -->
      <Card
        class="bg-gray-80 border-gray text-black rounded-sm lg:max-w-xs px-4 pt-8"
      >
        <CardContent class="flex flex-col items-center">
          <Avatar class="mb-4 max-w-xs w-64 h-64">
            <AvatarImage src={data.avatar} alt="User profile picture" />
            <AvatarFallback class="bg-secondary text-primary text-8xl"
              >{data.name
                .split(" ")
                .map((x) => x.substring(0, 1))
                .join("") ?? "CM"}</AvatarFallback
            >
          </Avatar>
          <span class="mt-4 mb-2 text-3xl">{data.name ?? "Car McCarface"}</span>
          <button
            class="mt-4 mb-2 px-4 py-2 rounded-sm bg-accent text-foreground border border-accent"
            onclick={handleChangeAvatar}>Change Avatar</button
          >
          <button
            class="mt-4 mb-2 px-4 py-2 rounded-sm bg-accent text-foreground border border-accent"
            onclick={handleChangePassword}>Change Password</button
          >
        </CardContent>
      </Card>
      <!--Edit Profile Card-->
      <Card class="bg-gray-80 border-gray text-black rounded-sm grow">
        <CardHeader class="flex flex-col items-center">
          <h2 class="flex items-center">Edit Profile</h2>
          <Separator class="mt-1.75" />
        </CardHeader>
        <CardContent>
          <div class="p-8 rounded-sm bg-gray gap-4">
            <div class="mb-2 grid gap-6 md:grid-cols-2">
              <div class="flex flex-col gap-2 w-full">
                <h1 class="mb-2">Name</h1>
                <Input
                  class="mb-4 border border-accent"
                  placeholder="Name"
                  {disabled}
                  bind:value={data.name}
                />
              </div>
              <div class="flex flex-col gap-2 w-full">
                <h1 class="mb-2">Email</h1>
                <Input
                  class="mb-4 border border-accent"
                  placeholder="Email"
                  {disabled}
                  bind:value={data.email}
                />
              </div>
              <div class="flex flex-col gap-2 w-full">
                <h1 class="mb-2">System</h1>
                <Input
                  class="mb-4 border border-accent"
                  placeholder="System"
                  {disabled}
                  bind:value={data.system}
                />
              </div>
              <div class="flex flex-col gap-2 w-full">
                <h1 class="mb-2">Subsystem</h1>
                <Input
                  class="mb-4 border border-accent"
                  placeholder="Subsystem"
                  {disabled}
                  bind:value={data.subsystem}
                />
              </div>
              <div class="flex flex-col gap-2 w-full">
                <h1 class="mb-2">Graduation Date</h1>
                <Input
                  class="mb-4 border border-accent"
                  placeholder="Graduation Date"
                  {disabled}
                  bind:value={data.gradDate}
                />
              </div>
              <div class="flex flex-col gap-2 w-full">
                <h1 class="mb-2">T-Shirt Size</h1>
                <Input
                  class="mb-4 border border-accent"
                  placeholder="T-Shirt Size"
                  {disabled}
                  bind:value={data.shirtSize}
                />
              </div>
              <div class="flex flex-col gap-2 w-full">
                <h1 class="mb-2">Hazing Status</h1>
                <Input
                  class="mb-4 border border-accent"
                  placeholder="Hazing Status"
                  disabled={true}
                  bind:value={data.hazingStatus}
                />
              </div>
              <div class="flex flex-col gap-2 w-full">
                <h1 class="mb-2">Fee Status</h1>
                <Input
                  class="mb-4 border border-accent"
                  placeholder="Fee Status"
                  disabled={true}
                  bind:value={data.feeStatus}
                />
              </div>
            </div>
            <div class="gap-4 flex">
              <button
                class="mt-4 px-4 py-2 rounded-sm bg-accent text-foreground border border-accent"
                onclick={isEditing ? handleSaveProfile : handleEditProfile}
              >
                {isEditing ? "Save" : "Edit"} Profile
              </button>
            </div>
          </div>
        </CardContent>
      </Card>
    </div>
  </div>
</div>
