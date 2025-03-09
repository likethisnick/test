import { 
    Box,
    Heading,  
    Text,  
} from "@chakra-ui/react"

export default function Note({title, description, createdAt}) {
    return (
        <Box color="black" p={4} borderWidth="1px">
        <Box mb={2} >
            <Heading size="md">{title}</Heading>
        </Box>
        <Box mb={2}>
            <Text>{description}</Text>
        </Box>
        <Box>
            <Text fontSize="sm">{createdAt}</Text>
        </Box>
        </Box>
    );
}