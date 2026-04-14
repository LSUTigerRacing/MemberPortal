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

  let user = $state<TRAPI.User | null>(null);
  let uploading = $state(false);
  let files = $state<FileList | null>(null);

  let feeStatus = $state<"Unpaid" | "Paid" | "Waived">("Unpaid");
  let hazingStatus = $state<"Completed" | "Uncompleted" | "Waived">("Uncompleted");

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

  let data = $state<ProfileFormState>({
    name: "Car McCarface",
    email: "cmccarface1@lsu.edu",
    avatar: "",
    system: "Chassis",
    subsystem: "Battery",
    gradDate: "2026",
    shirtSize: "L",
    hazingStatus: false,
    feeStatus: true,
    });

  let tempData = $state<ProfileFormState>({ 
    name: "Car McCarface",
    email: "cmccarface1@lsu.edu",
    avatar: "",
    system: "Chassis",
    subsystem: "Battery",
    gradDate: "2026",
    shirtSize: "L",
    hazingStatus: false,
    feeStatus: true,
   });

  $effect(() => {
    (async () => {
      // To do: Make this fetch the current user's ID
      const res = await api.fetchUser();
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
      tempData = {...data};

      if (tempData.hazingStatus === true) {
        hazingStatus = "Completed";
      } else if (tempData.hazingStatus === false) {
        hazingStatus = "Waived";
      } else {
        hazingStatus = "Uncompleted";
      }

      if (tempData.feeStatus === true) {
        feeStatus = "Paid";
      } else if (tempData.feeStatus === false) {
        feeStatus = "Unpaid";
      } else {
        feeStatus = "Waived";
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
    const gradYear = Number.parseInt(tempData.gradDate, 10);
    if (user?.id) {
      await api.updateUser(user.id, {
        name: tempData.name,
        email: tempData.email,
        avatar: tempData.avatar,
        system: tempData.system ? (tempData.system as TRAPI.User["system"]) : undefined,
        subsystem: tempData.subsystem
          ? (tempData.subsystem as TRAPI.User["subsystem"])
          : undefined,
        gradYear: Number.isNaN(gradYear) ? undefined : gradYear,
        shirtSize: tempData.shirtSize
          ? (tempData.shirtSize as TRAPI.User["shirtSize"])
          : undefined,
        hazingStatus: tempData.hazingStatus,
        feeStatus: tempData.feeStatus,
      });
    } else {
      data = {...tempData};
    }
  }

  async function handleCancelEditProfile() {
    disabled = true;
    isEditing = false;
    if (user?.id) {
      tempData.name = user.name;
      tempData.email = user.email;
      tempData.avatar = user.avatar ?? "";
      tempData.system = user.system ?? "";
      tempData.subsystem = user.subsystem ?? "";
      tempData.gradDate = user.gradYear?.toString() || "";
      tempData.shirtSize = user.shirtSize ?? "";
      tempData.hazingStatus = user.hazingStatus;
      tempData.feeStatus = user.feeStatus;
    } else if (user === null) {
      tempData = {...data};
    }
  }

  async function handleChangeAvatar() {
    try {
      uploading = true; 
      const file = files?.[0] ?? null;

      if (!file) {
        alert("No file selected");
        return;
      }

      tempData.avatar = await new Promise<string>((resolve, reject) => {
        const reader = new FileReader();
        reader.onload = () => resolve(reader.result as string);
        reader.onerror = (error) => reject(error);
        reader.readAsDataURL(file);
      });
    } finally {
      uploading = false;
    }
  }

  function handleChangePassword() {
    alert("Change password functionality is not implemented yet.");
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
        class="bg-primary border-gray text-black rounded-sm lg:max-w-xs px-4 pt-8"
      >
        <CardContent class="flex flex-col items-center">
          <Avatar class="mb-4 max-w-xs w-64 h-64">
            <AvatarImage src={tempData.avatar} alt="User profile picture" />
            <AvatarFallback class="bg-secondary text-primary text-8xl"
              >{tempData.name
                .split(" ")
                .map((x) => x.substring(0, 1))
                .join("") ?? "CM"}</AvatarFallback
            >
          </Avatar>
          <span class="mt-4 mb-2 w-full text-3xl text-center break-words text-white">{tempData.name ?? "Car McCarface"}</span>
          <input
            type="file"
            id="single"
            accept="image/*"
            bind:files={files}
            disabled={uploading}
            class="hidden"
            onchange={handleChangeAvatar}
          >
          <label for="single" class="mt-4 mb-2 px-4 py-2 rounded-sm bg-accent text-foreground border border-accent cursor-pointer">
            {uploading ? "Uploading..." : "Change Avatar"}
          </label>
          <button
            class="mt-4 mb-2 px-4 py-2 rounded-sm bg-accent text-foreground border border-accent"
            onclick={handleChangePassword}>Change Password</button
          >
        </CardContent>
      </Card>
      <!--Edit Profile Card-->
      <Card class="bg-primary border-gray text-white rounded-sm grow">
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
                  class="mb-4 border border-accent text-black"
                  placeholder="Name"
                  {disabled}
                  bind:value={tempData.name}
                />
              </div>
              <div class="flex flex-col gap-2 w-full">
                <h1 class="mb-2">Email</h1>
                <Input
                  class="mb-4 border border-accent text-black"
                  placeholder="Email"
                  {disabled}
                  bind:value={tempData.email}
                />
              </div>
              <div class="flex flex-col gap-2 w-full">
                <h1 class="mb-2">System</h1>
                <Input
                  class="mb-4 border border-accent text-black"
                  placeholder="System"
                  {disabled}
                  bind:value={tempData.system}
                />
              </div>
              <div class="flex flex-col gap-2 w-full">
                <h1 class="mb-2">Subsystem</h1>
                <Input
                  class="mb-4 border border-accent text-black"
                  placeholder="Subsystem"
                  {disabled}
                  bind:value={tempData.subsystem}
                />
              </div>
              <div class="flex flex-col gap-2 w-full">
                <h1 class="mb-2">Graduation Date</h1>
                <Input
                  class="mb-4 border border-accent text-black"
                  placeholder="Graduation Date"
                  {disabled}
                  bind:value={tempData.gradDate}
                />
              </div>
              <div class="flex flex-col gap-2 w-full">
                <h1 class="mb-2">T-Shirt Size</h1>
                <Input
                  class="mb-4 border border-accent text-black"
                  placeholder="T-Shirt Size"
                  {disabled}
                  bind:value={tempData.shirtSize}
                />
              </div>
              <div class="flex flex-col gap-2 w-full">
                <h1 class="mb-2">Hazing Modules</h1>
                <Input
                  class="mb-4 border border-accent text-black"
                  placeholder="Hazing Modules"
                  disabled={true}
                  bind:value={hazingStatus}
                />
              </div>
              <div class="flex flex-col gap-2 w-full">
                <h1 class="mb-2">Fee Status</h1>
                <Input
                  class="mb-4 border border-accent text-black"
                  placeholder="Fee Status"
                  disabled={true}
                  bind:value={feeStatus}
                />
              </div>
            </div>
            <div class="gap-4 flex">
            {#if isEditing}
              <button
                class="mt-4 px-4 py-2 rounded-sm bg-accent text-black border border-accent"
                onclick={handleCancelEditProfile}
              >
                Cancel
              </button>
            {/if}
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
