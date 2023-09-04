import Skeleton from "@mui/material/Skeleton";
import Stack from "@mui/material/Stack";

// Loading skeleton for forms
const FormLoadingSkeleton = () => {
  return (
    <Stack spacing={2}>
      <Skeleton variant="rounded" width={300} height={75} />
      <Skeleton variant="rounded" height={100} />
      <Skeleton variant="rounded" height={100} />
      <Skeleton variant="rounded" height={100} />
      <Skeleton variant="rounded" height={100} />
      <Skeleton variant="rounded" height={100} />
      <Skeleton variant="rounded" height={100} />
      <div className="flex justify-end">
        <Skeleton variant="rounded" width={100} height={50} />
      </div>
    </Stack>
  );
};

export default FormLoadingSkeleton;
