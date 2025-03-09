import React from "react";
import avatarImage from './SiteLogo/Logo.jpg'
import { Avatar, For, HStack } from "@chakra-ui/react";

export default function Logo(props) {
    return (
        <HStack gap="3">
          <For each={["2xl"]}>
            {(size) => (
              <Avatar.Root size={size} key={size}>
                <Avatar.Fallback name="Segun Adebayo" />
                <Avatar.Image src={avatarImage} />
              </Avatar.Root>
            )}
          </For>
        </HStack>
      )
}