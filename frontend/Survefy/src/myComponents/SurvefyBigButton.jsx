import { 
    Box,
    Heading,  
    Text,  
} from "@chakra-ui/react"
import { Button } from "@mui/material";

export default function SurvefyBigButton({title, margin = 2}) {
    return (
        <Button 
        sx={{
          fontSize: '1.5rem',
          padding: '16px 24px',
          maxWidth: 255,
          minHeight: 55,
          backgroundColor: '#7DABFF',
          color: '#fff',
          transition: 'background-color 0.3s',
          mb: margin,
          '&:hover': {
            backgroundColor: '#115293',
          },
        }}
      >
        {title}
      </Button>
    );
}